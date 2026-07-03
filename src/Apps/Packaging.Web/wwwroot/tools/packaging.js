(function () {
    const body = document.body;
    const statusText = document.getElementById("statusText");
    const authUser = document.getElementById("authUser");
    const loginForm = document.getElementById("loginForm");
    const usernameInput = document.getElementById("usernameInput");
    const passwordInput = document.getElementById("passwordInput");
    const loginButton = document.getElementById("loginButton");
    const logoutButton = document.getElementById("logoutButton");
    const packagesGrid = document.getElementById("packagesGrid");
    const createPackageButton = document.getElementById("createPackageButton");
    const editorDialog = document.getElementById("editorDialog");
    const editorTitle = document.getElementById("editorTitle");
    const editorFields = document.getElementById("editorFields");
    const saveDialogButton = document.getElementById("saveDialogButton");
    const closeDialogButton = document.getElementById("closeDialogButton");

    const packageFields = ["Id", "Name", "Description", "Category", "SourceApi"];
    const packageItemFields = ["Id", "PackageId", "Type", "Data"];

    let packages = [];
    let packageItemsByPackageId = new Map();
    let editor = null;

    function setStatus(message, isError) {
        statusText.textContent = message;
        statusText.classList.toggle("packaging-status-error", Boolean(isError));
    }

    function updateAuthState() {
        const isAuthenticated = Boolean(window.packagingApi.getToken());
        body.classList.toggle("is-authenticated", isAuthenticated);
        authUser.textContent = window.packagingApi.getUser();
        loginButton.hidden = isAuthenticated;
        logoutButton.hidden = !isAuthenticated;
        usernameInput.hidden = isAuthenticated;
        passwordInput.hidden = isAuthenticated;

        if (isAuthenticated) {
            refresh().catch(error => setStatus(error.message, true));
        }
    }

    function odataRows(result) {
        return Array.isArray(result && result.value)
            ? result.value
            : Array.isArray(result)
                ? result
                : [];
    }

    async function loadPackages() {
        const result = await window.packagingApi.request(
            "/Api/Packaging/Package?$orderby=Name");

        packages = odataRows(result);
    }

    async function loadPackageItems(packageId) {
        if (packageItemsByPackageId.has(packageId)) {
            return packageItemsByPackageId.get(packageId);
        }

        const result = await window.packagingApi.request(
            `/Api/Packaging/PackageItem?$filter=PackageId eq ${packageId}&$orderby=Type`);
        const items = odataRows(result);
        packageItemsByPackageId.set(packageId, items);

        return items;
    }

    async function refresh() {
        setStatus("Loading");
        packageItemsByPackageId = new Map();
        await loadPackages();
        renderPackages();
        setStatus("Ready");
    }

    function renderPackages() {
        const rows = packages.map(packageRow => `
            <tr data-package-id="${escapeHtml(packageRow.Id)}">
                <td class="packaging-expand-column">
                    <button class="packaging-expand-toggle" data-action="toggle-package" type="button" aria-label="Expand package">
                        <span class="packaging-expand-icon"></span>
                    </button>
                </td>
                <td>${escapeHtml(packageRow.Name)}</td>
                <td>${escapeHtml(packageRow.Description)}</td>
                <td>${escapeHtml(packageRow.Category)}</td>
                <td>${escapeHtml(packageRow.SourceApi)}</td>
                <td>${escapeHtml(packageRow.Id)}</td>
                <td class="packaging-actions">
                    <button data-action="edit-package" type="button">Edit</button>
                    <button data-action="delete-package" type="button">Delete</button>
                </td>
            </tr>`);

        packagesGrid.innerHTML = `
            <table class="packaging-table">
                <thead>
                    <tr>
                        <th class="packaging-expand-column"></th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Category</th>
                        <th>Source Api</th>
                        <th>Id</th>
                        <th class="packaging-actions">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    ${rows.join("") || `<tr><td class="packaging-empty" colspan="7">No Package rows found.</td></tr>`}
                </tbody>
            </table>`;
    }

    async function togglePackage(row) {
        const nextRow = row.nextElementSibling;
        const button = row.querySelector("[data-action='toggle-package']");

        if (nextRow && nextRow.classList.contains("packaging-detail-row")) {
            nextRow.remove();
            button.classList.remove("expanded");
            button.setAttribute("aria-label", "Expand package");
            return;
        }

        const packageRow = findPackage(row.dataset.packageId);
        button.classList.add("expanded");
        button.setAttribute("aria-label", "Collapse package");

        const detailRow = document.createElement("tr");
        detailRow.className = "packaging-detail-row";
        detailRow.innerHTML = `<td colspan="7">${renderPackageItemsShell(packageRow)}</td>`;
        row.insertAdjacentElement("afterend", detailRow);

        await renderPackageItems(detailRow, packageRow);
    }

    function renderPackageItemsShell(packageRow) {
        return `
            <div class="packaging-detail">
                <div class="packaging-tabs">
                    <button class="active" data-detail-tab="items" type="button">Package Items</button>
                </div>
                <section class="packaging-tab-panel active" data-detail-panel="items">
                    <div class="packaging-detail-toolbar">
                        <button data-action="create-package-item" data-package-id="${escapeHtml(packageRow.Id)}" type="button">Create Package Item</button>
                    </div>
                    <div data-child-grid="PackageItem"></div>
                </section>
            </div>`;
    }

    async function renderPackageItems(detailRow, packageRow) {
        const items = await loadPackageItems(packageRow.Id);
        const rows = items.map(item => `
            <tr data-package-id="${escapeHtml(packageRow.Id)}" data-package-item-id="${escapeHtml(item.Id)}">
                <td>${escapeHtml(item.Type)}</td>
                <td><pre>${escapeHtml(item.Data)}</pre></td>
                <td>${escapeHtml(item.Id)}</td>
                <td class="packaging-actions">
                    <button data-action="edit-package-item" type="button">Edit</button>
                    <button data-action="delete-package-item" type="button">Delete</button>
                </td>
            </tr>`);

        detailRow.querySelector("[data-child-grid='PackageItem']").innerHTML = `
            <table class="packaging-table">
                <thead>
                    <tr>
                        <th>Type</th>
                        <th>Data</th>
                        <th>Id</th>
                        <th class="packaging-actions">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    ${rows.join("") || `<tr><td class="packaging-empty" colspan="4">No Package Item rows found.</td></tr>`}
                </tbody>
            </table>`;
    }

    function findPackage(id) {
        return packages.find(packageRow => sameId(packageRow.Id, id));
    }

    function findPackageItem(packageId, itemId) {
        const items = packageItemsByPackageId.get(packageId) || [];

        return items.find(item => sameId(item.Id, itemId));
    }

    function openPackageEditor(packageRow) {
        editor = {
            title: packageRow ? "Edit Package" : "Create Package",
            fields: packageFields,
            value: {
                Id: packageRow ? packageRow.Id : crypto.randomUUID(),
                Name: packageRow ? packageRow.Name : "",
                Description: packageRow ? packageRow.Description : "",
                Category: packageRow ? packageRow.Category : "",
                SourceApi: packageRow ? packageRow.SourceApi : ""
            },
            save: savePackage
        };

        openEditor();
    }

    function openPackageItemEditor(packageId, item) {
        editor = {
            title: item ? "Edit Package Item" : "Create Package Item",
            fields: packageItemFields,
            value: {
                Id: item ? item.Id : crypto.randomUUID(),
                PackageId: packageId,
                Type: item ? item.Type : "",
                Data: item ? item.Data : ""
            },
            save: savePackageItem
        };

        openEditor();
    }

    function openEditor() {
        editorTitle.textContent = editor.title;
        editorFields.innerHTML = editor.fields.map(field => {
            const value = editor.value[field] || "";
            const readonly = field === "Id" || field === "PackageId" ? " readonly" : "";

            return field === "Data"
                ? `<label><span>${field}</span><textarea data-field="${field}">${escapeHtml(value)}</textarea></label>`
                : `<label><span>${field}</span><input data-field="${field}" value="${escapeAttribute(value)}"${readonly}></label>`;
        }).join("");
        editorDialog.showModal();
    }

    async function savePackage(value) {
        const isExisting = packages.some(packageRow => sameId(packageRow.Id, value.Id));
        await window.packagingApi.request(
            isExisting
                ? `/Api/Packaging/Package(${value.Id})`
                : "/Api/Packaging/Package",
            {
                method: isExisting ? "PUT" : "POST",
                body: JSON.stringify({ ...value, Items: [] })
            });
    }

    async function savePackageItem(value) {
        const items = packageItemsByPackageId.get(value.PackageId) || [];
        const isExisting = items.some(item => sameId(item.Id, value.Id));

        await window.packagingApi.request(
            isExisting
                ? `/Api/Packaging/PackageItem(${value.Id})`
                : "/Api/Packaging/PackageItem",
            {
                method: isExisting ? "PUT" : "POST",
                body: JSON.stringify(value)
            });
    }

    async function deletePackage(packageRow) {
        if (!confirm(`Delete package '${packageRow.Name}'?`)) {
            return;
        }

        await window.packagingApi.request(`/Api/Packaging/Package(${packageRow.Id})`, {
            method: "DELETE"
        });
    }

    async function deletePackageItem(item) {
        if (!confirm(`Delete package item '${item.Type}'?`)) {
            return;
        }

        await window.packagingApi.request(`/Api/Packaging/PackageItem(${item.Id})`, {
            method: "DELETE"
        });
    }

    function collectEditorValue() {
        const value = {};

        editorFields.querySelectorAll("[data-field]").forEach(input => {
            value[input.dataset.field] = input.value;
        });

        return value;
    }

    loginForm.addEventListener("submit", event => {
        event.preventDefault();
        window.packagingApi.login(usernameInput.value, passwordInput.value)
            .then(() => {
                usernameInput.value = "";
                passwordInput.value = "";
                updateAuthState();
            })
            .catch(error => setStatus(error.message, true));
    });

    logoutButton.addEventListener("click", () => {
        window.packagingApi.setToken(null);
        updateAuthState();
    });

    createPackageButton.addEventListener("click", () => openPackageEditor(null));

    closeDialogButton.addEventListener("click", () => editorDialog.close());

    saveDialogButton.addEventListener("click", () => {
        editor.save(collectEditorValue())
            .then(() => {
                editorDialog.close();
                return refresh();
            })
            .catch(error => setStatus(error.message, true));
    });

    packagesGrid.addEventListener("click", event => {
        const button = event.target.closest("button[data-action]");

        if (!button) {
            return;
        }

        const row = button.closest("tr");
        const action = button.dataset.action;
        const packageRow = row ? findPackage(row.dataset.packageId) : null;

        if (action === "toggle-package") {
            togglePackage(row)
                .catch(error => setStatus(error.message, true));
            return;
        }

        if (action === "edit-package") {
            openPackageEditor(packageRow);
            return;
        }

        if (action === "delete-package") {
            deletePackage(packageRow)
                .then(refresh)
                .catch(error => setStatus(error.message, true));
            return;
        }

        if (action === "create-package-item") {
            openPackageItemEditor(button.dataset.packageId, null);
            return;
        }

        if (action === "edit-package-item") {
            openPackageItemEditor(row.dataset.packageId, findPackageItem(row.dataset.packageId, row.dataset.packageItemId));
            return;
        }

        if (action === "delete-package-item") {
            deletePackageItem(findPackageItem(row.dataset.packageId, row.dataset.packageItemId))
                .then(refresh)
                .catch(error => setStatus(error.message, true));
        }
    });

    function sameId(left, right) {
        return String(left || "").toLowerCase() === String(right || "").toLowerCase();
    }

    function escapeHtml(value) {
        return String(value || "")
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function escapeAttribute(value) {
        return escapeHtml(value).replace(/`/g, "&#096;");
    }

    updateAuthState();
})();

(function () {
    const tokenKey = "ccoder.packaging.token";
    const userKey = "ccoder.packaging.user";

    function getToken() {
        return sessionStorage.getItem(tokenKey);
    }

    function setToken(token, user) {
        if (token) {
            sessionStorage.setItem(tokenKey, token);
            sessionStorage.setItem(userKey, user || "Authenticated");
        } else {
            sessionStorage.removeItem(tokenKey);
            sessionStorage.removeItem(userKey);
        }
    }

    function getUser() {
        return sessionStorage.getItem(userKey) || "Guest";
    }

    async function request(url, options) {
        const headers = new Headers(options && options.headers ? options.headers : {});
        const token = getToken();

        if (token) {
            headers.set("Authorization", `Bearer ${token}`);
        }

        if (options && options.body && !headers.has("Content-Type")) {
            headers.set("Content-Type", "application/json");
        }

        const response = await fetch(url, {
            ...(options || {}),
            headers
        });

        if (!response.ok) {
            const error = await response.text();
            throw new Error(error || response.statusText);
        }

        if (response.status === 204) {
            return null;
        }

        const contentType = response.headers.get("Content-Type") || "";
        return contentType.includes("application/json")
            ? response.json()
            : response.text();
    }

    async function login(username, password) {
        const result = await request("/Api/Account/Login", {
            method: "POST",
            body: JSON.stringify({
                user: username,
                pass: password
            })
        });

        const token = result.token || result.Token || result.id || result.Id;

        if (!token) {
            throw new Error("Login did not return a token.");
        }

        setToken(token, username);
        return result;
    }

    window.packagingApi = {
        getToken,
        setToken,
        getUser,
        request,
        login
    };
})();

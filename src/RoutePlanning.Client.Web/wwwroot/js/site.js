window.localStorageFunctions = {
    setItem: function (key, data) {
        localStorage.setItem(key, data);
    },
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    }
};

window.showWarningFunctions = {
    displayLoginRequire: function () {
        window.alert("Please login!");
    },
    displayRegisterSuccess: function () {
        window.alert("Created a booking successfully!");
    }
}
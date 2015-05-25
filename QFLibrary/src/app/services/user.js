qflibrary.factory('User', function ($cookies) {

    function getUser() {
        var cookieDecrypted,
            cookieString,
            cookie;

        cookieDecrypted = getCookie();
        cookieString = decrypt(cookieDecrypted);
        cookie = parse(cookieString);

        return toUser(cookie);
    }

    function toUser(cookie) {
        return {
            id: cookie.account
        };
    }

    function parse(str) {
        return JSON.parse(str);
    }

    function decrypt(str) {
        return atob(str);
    }

    function getCookie() {
        return $cookies['qflibrary_session'];
    }

    return getUser();
});
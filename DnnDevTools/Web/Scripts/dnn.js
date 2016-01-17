(function () {
    dnnDevTools = {
        ajax: function (method, url, success, error) {
            var req = new XMLHttpRequest();

            req.open(method, url);

            req.setRequestHeader('requestVerificationToken', $.ServicesFramework().getAntiForgeryValue());

            req.onload = function () {
                var response = (req.responseText && req.responseText != '') ? JSON.parse(req.responseText) : '';

                if (req.status == 200) {
                    success(response);
                }
                else {
                    error(Error(req.statusText));
                }
            };

            req.onerror = function () {
                error(Error("Please check your internet connection."));
            };

            req.send();
        }
    }
}());
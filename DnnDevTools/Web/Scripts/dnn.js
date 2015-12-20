(function () {
    dnnMdt = {
        ajax: function (method, url) {
            return new Promise(function (resolve, reject) {
                var req = new XMLHttpRequest();

                req.open(method, url);

                req.setRequestHeader('requestVerificationToken', $.ServicesFramework().getAntiForgeryValue());

                req.onload = function () {
                    if (req.status == 200) {
                        resolve(req.response);
                    }
                    else {
                        reject(Error(req.statusText));
                    }
                };

                req.onerror = function () {
                    reject(Error("Please check your internet connection."));
                };

                req.send();
            });
        }
    }
}());
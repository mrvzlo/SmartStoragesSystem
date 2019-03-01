// ReSharper disable VariableUsedInInnerScopeBeforeDeclared
var AccountIndexJs = (function () {

    var settings = {};

    var initialize = function (options) {

        var defaults = {
            signInForm: "#formSignIn",
            signUpForm: "#formSignUp",
            signInContainer: "#_SignIn",
            signUpContainer: "#_SignUp"
        };
        settings = $.extend(true, defaults, options);

        FormHelperJs.bindForm(settings.signInForm, settings.signInContainer);
        FormHelperJs.bindForm(settings.signUpForm, settings.signUpContainer);
    }
    return {
        initialize: initialize
    };

})();
import { ErrorMessages } from './ErrorMessages';

declare var $:any; // "mock" jquery

export class Default {
    
    public ErrorMessages: ErrorMessages;
    
    constructor() {
        this.ErrorMessages = new ErrorMessages(); 
    }
    // @param email = email to validate
    // @return bool
    validateEmail(email: string) {
        var regex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return regex.test(email);
    }
    
    validatePassword(password: string, id: string) {
            if (/^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$/.test(password) === false) {
                this.addError(id, "Invalid password.");
                return false;
            } else {
                this.removeError(id);
            }
            return true;
    }
    
    addError (selector: string, message: string) : void {
        $(selector).addClass("has-error");
        let $nextElem:any = $(selector).next();
        if (!$nextElem.hasClass("error-message"))   {
            $(selector).after("<div class=\"error-message\"><p>" + message + "</p></div>");            
        }     
    }

    removeError (selector: string): void {
        $(selector).removeClass("has-error");
        let $nextElem:any = $(selector).next();
        if ($nextElem.hasClass("error-message")) {
            $nextElem.remove();
        }
    }
    
    clearErrors(formID: string = undefined) {
        $(".has-error").removeClass("has-error");
        $(".error-message").remove();
        if (formID != undefined) {
            var form = document.getElementById(formID);
            form.reset();
        }
    }
    
    clearModalErrors(modal: string) {
        $(modal).on('hidden.bs.modal', function () {
            var id = $(this).find('form').attr('id');
            this.clearErrors(id);
        });
    }
    
    updateInputField(field: string) {
        $(field).change(function () {
            if (this.value != "") {
                $(this).removeClass("input-error");
                $("." + field.substring(1) + ".error-message").remove();
            }
        });
    }
    
    get(settings: Object, async: boolean = true) {   
        if (typeof(settings) === 'object') {
            var baseSettings = {
                "type"       : 'GET',
                "contentType": "application/json", 
                "async"      : async
            };
            var mergedSettings = $.extend(baseSettings, settings)
            $.ajax(mergedSettings);
            return true;
        }
        return false;
    }
    
    post(settings: Object, async: boolean = true) {   
        if (typeof(settings) === 'object') {
            var baseSettings = {
                "type"       : 'POST',
                "contentType": "application/json", 
                "async"      : async
            };
            var mergedSettings = $.extend(baseSettings, settings)
            $.ajax(mergedSettings);
            return true;
        }
        return false;
    }
    
    alertMsg(type: string, action: string, msgID: string) {
        if (type == undefined) return;
        var deleteMsg = '<div class="alert alert-success alert-dismissible" role="alert">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<strong>Success!</strong> ' + type + ' has been deleted.' +
            '</div>';
        var updateMsg = '<div class="alert alert-success alert-dismissible" role="alert">' +
            '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
            '<strong>Success!</strong> ' + type + ' has been updated.' +
            '</div>';
        if (action == 'delete') $(msgID).html(deleteMsg);
        if (action == 'update') $(msgID).html(updateMsg);
    }
}
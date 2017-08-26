import { ErrorMessages } from './error.messages';

declare var $:any; // "mock" jquery

export class Common {
    
    public errorMessages: ErrorMessages;
    
    constructor() {
        this.errorMessages = new ErrorMessages(); 
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

    shallowCopy(obj:any): any{
        let copy = Object.create(obj);
        for (let prop of obj) {
            if (obj.hasOwnProperty(prop)) {
                copy[prop] = obj[prop];
            }
        }
        return copy;
    }

    parseJsonObject(objString:string): any {
        let resultObj:any = null;
        
        try {
            resultObj = JSON.parse(objString);
        } catch (ex) {
            console.log(ex);
        }

        return resultObj;
    }
    
    hasProperties(obj:any, propertyNames:Array<string>): boolean {
        let validObj = true;
        if (typeof(obj) === "object" && obj != null) {
            propertyNames.some(name => {
                validObj = obj.hasOwnProperty(name);
                return !validObj;
            });
        }
        return validObj;
    }

    clearHash(): void {
        let currentHash = window.location.hash;
        if (typeof (currentHash) === "string" && currentHash.length > 0) {
            window.location.hash = "";
        }
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
        if ($nextElem != undefined && $nextElem.hasClass("error-message")) {
            $nextElem.remove();
        }
    }
    
    clearErrors(formID: string = undefined) {
        $(".has-error").removeClass("has-error");
        $(".error-message").remove();
        if (formID != undefined) {
            var form = document.getElementById(formID);
            //form.reset();
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

    // validates that a string is a guid
    isGuid(stringToTest: string): boolean {
        if (stringToTest[0] === "{") {
            stringToTest = stringToTest.substring(1, stringToTest.length - 1);
        }
        let regexGuid: RegExp = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
        return regexGuid.test(stringToTest);
    }

    getQueryStringParam(name: string): string {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        let regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        let results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
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
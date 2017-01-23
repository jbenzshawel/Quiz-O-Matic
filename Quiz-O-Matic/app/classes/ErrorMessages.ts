export class ErrorMessages {

    public PasswordLength           : string; 
    
    public IncorrectOldPassword     : string;
    
    public PasswordNotMatch         : string;
    
    public InvalidNumber            : string;
    
    public NameLength               : string;
    
    public Email                    : string;
    
    public Title                    : string;
    
    public Content                  : string;
    
    public InvalidUsernameOrPassword: string;
    
    constructor() {
        this.PasswordLength            = "Password must be at least 5 characters long";
        this.IncorrectOldPassword      = "Old password is not correct";
        this.PasswordNotMatch          = "Confirm password does not match new password";
        this.InvalidNumber             = "&nbsp;&nbsp;You did not enter a number";
        this.NameLength                = "The name field must be at least 3 characters long";
        this.Email                     = "The email must be of the format address@example.com";
        this.Title                     = "The title field is required";
        this.Content                   = "The content field is required";
        this.InvalidUsernameOrPassword = "Invalid Username or Password";
        
    }
};
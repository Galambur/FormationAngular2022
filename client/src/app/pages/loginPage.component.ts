import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";

@Component({
    selector: "login-page",
    templateUrl: "loginPage.component.html"
})
export default class LoginPage {
    constructor(public store: Store, private router: Router) { }

    public creds = {
        username: "",
        password: ""
    }

    public onLogin() {
        alert("Logging");
    }
}
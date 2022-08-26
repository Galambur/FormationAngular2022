import { Component } from "@angular/core";

@Component({
  selector: "checkout",
  templateUrl: "checkout.component.html",
  styleUrls: ['checkout.component.css']
})
export class Checkout {

  constructor() {
  }

  onCheckout() {
    // TODO
    alert("Doing checkout");
  }
}
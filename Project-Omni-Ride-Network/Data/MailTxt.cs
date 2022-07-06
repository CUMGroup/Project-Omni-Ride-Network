using System;

namespace Project_Omni_Ride_Network {
    public static class MailTxt {
        // class that handles the mail texts that need to be send

        #region constantTexts
        // constant mail texts

        private const string SALUTATIONS = "<html><body><p> Hallo ";

        private const string SERVICE_RESP = ", <br><br>" +
            "vielen Dank, dass Sie uns schreiben! <br> " +
            "Wir werden uns darum Bemühen Ihr Anliegen schnellstmöglich zu bearbeiten. <br> " +
            "Bis dahin eine gute Fahrt! <br><br>" +
            "Ihr Car Utility Management Team <br>" +
            "------------------------------- <br>" +
            "We are MOBILITY <br>" +
            "We are INNOVATION <br>" +
            "We are LIBERTY <br>" +
            "We are FUTURE <br>" +
            "</p></body></html>";

        private const string REGISTRY_RESP = ", <br><br>" +
            "vielen Dank, dass Sie sich für uns entschieden haben! <br> " +
            "Ihre Registrierung ist hiermit abgeschlossen. <br> " +
            "Wir wünschen Ihnen eine gute Fahrt! <br><br>" +
            "Ihr Car Utility Management Team <br> " +
            "------------------------------- <br>" +
            "We are MOBILITY <br>" +
            "We are INNOVATION <br>" +
            "We are LIBERTY <br>" +
            "We are FUTURE <br>" +
            "</p></body></html>";

        private const string ORDER_RESP_START = ", <br><br>" +
            "vielen Dank für Ihre Bestellung über C.U.M! <br> " +
            "Hier die Details Ihrer Bestellung: <br><br> ";

        private const string ORDER_RESP_END = "<br><br>Wir wünschen Ihnen eine gute Fahrt! <br><br>" +
            "Ihr Car Utility Management Team <br> " +
            "------------------------------- <br>" +
            "We are MOBILITY <br>" +
            "We are INNOVATION <br>" +
            "We are LIBERTY <br>" +
            "We are FUTURE <br>" +
            "</p></body></html>";

        private const string ORDER_SUBJECT = "Ihre Bestellung: ";

        public const string REGISTRY_SUBJ = "Ihre Registrierung";

        #endregion

        #region createMails
        // Methods that create mails for specific reasons

        // register confirmation mail
        public static string CreateRegistryResponse(string anrede, string surname) {
            string mail = SALUTATIONS + anrede + " " + surname + REGISTRY_RESP;
            return mail;
        }

        // service response mail
        public static string CreateServiceResponse(string name) {
            string mail = SALUTATIONS + name + SERVICE_RESP;
            return mail;
        }

        // subject for order confirmation mail
        public static string CreateOrderSubject(Order order) {

            string subject = ORDER_SUBJECT + order.OrderId;

            return subject;

        }

        // order confirmation mail
        public static string CreateOrderResponse(Order order) {

            string mail = SALUTATIONS + order.User.KdTitle + " " + order.User.KdSurname + ORDER_RESP_START +
                "Ihre Order-ID: " + order.OrderId + "<br>" +
                "Ihr Fahrzeug: " + order.Vehicle.Brand + " " + order.Vehicle.Model + "<br>" +
                "Ihre Nutzungszeit: von " + order.DateTimePickUp.ToShortDateString() + " bis " + order.DateTimeReturn.ToShortDateString() + "<br>" +
                "Gesamtpreis: " + Math.Round(order.Totalprice ,2) + " €" +
                ORDER_RESP_END;


            return mail;
        }

        #endregion

    }
}

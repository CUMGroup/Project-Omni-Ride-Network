﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Omni_Ride_Network {
    public class MailTxt {

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
            "Hier die Details Ihrer Bestellung: <br> ";

        private const string ORDER_RESP_END = "<br><br>Wir wünschen Ihnen eine gute Fahrt! <br><br>" +
            "Ihr Car Utility Management Team <br> " +
            "------------------------------- <br>" +
            "We are MOBILITY <br>" +
            "We are INNOVATION <br>" +
            "We are LIBERTY <br>" +
            "We are FUTURE <br>" +
            "</p></body></html>";

        public const string REGISTRY_SUBJ = "Ihre Registrierung";

        public string createRegistryResponse(string anrede, string surname) {
            string mail = SALUTATIONS + anrede + " " + surname + REGISTRY_RESP;
            return mail;
        }

        public string createServiceResponse(string name) {
            string mail = SALUTATIONS + name + SERVICE_RESP;
            return mail;
        }



    }
}

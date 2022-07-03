
<p align="center">
<img src="https://user-images.githubusercontent.com/90132658/176987943-81ee9995-4dd6-4807-907c-69349cb44fe6.png " width="200" height="200" />
</p>


# Project-Omni-Ride-Network(Car Utility Management)  [Unsere Webseite](https://c-u-m.azurewebsites.net/)



Ein Projektteam bezüglisch der **Webengenireeing** [Project-Omni-Ride-Network](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/tree/master/Project-Omni-Ride-Network) and **Projektmanagment** [Projektmanagement](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/tree/master/Projektmanagement) Vorlessungen 
an der Dualen Hochschule Baden-Württemberg Karlsruhe.[
karlsruhe.dhbw.de/](karlsruhe.dhbw.de/).

Car Utility Manager is ein Carsharing Anbieter.  Kund*innen schließen mit dem Anbieter bei der Anmeldung einen Rahmenvertrag. Danach können sie alle Fahrzeuge des Anbieters rund um die Uhr selbstständig buchen. 

Viel Spaß und gute Fahrt :red_car: :motorcycle:

## Dokumentation 
[Fachkonzept Porn](https://github.com/Annalytic-programming/Project-Omni-Ride-Network/blob/master/Projektmanagement/Fachkonzept%20PORN.pdf)

## Use it locally

Um auch die Code in Ihr system nutzen zu können

Um create die Webseite locally von current branch, 

``` #clone the project
 
git clone https://github.com/Annalytic-programming/Project-Omni-Ride-Network.git


```

## Konfigurationseinstellungen für Identity Server

Fügen Sie die appsettings.json-Datei im Stammverzeichnis des Projekts nach diesem Muster: 

``` {
    "MailCredentials": {
        "Hostname": "HOSTNAME",
        "Port": PORT,
        "Email": "EMAIL",
        "Passwort": "PASSWORD"
    },

    "ConnectionStrings": {
        "DefaultConnection": "CONNECTION STRING FOR DATABASE"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "JWT": {
        "Audience": "AUDIENCE",
        "Issuer": "ISSUER",
        "Secret": "SECRET"
    },
    "AllowedHosts": "*"

}
``` 




## Unser Team

**Frontend-Developers:** : Jasmin Hübner- Elnaz Gharoon

**Backend- Developers:** Anna Weber- Alexander Hagl 


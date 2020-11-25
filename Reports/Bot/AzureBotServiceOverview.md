# Azure Bot Service

1. **Intro**
   1. Jest to serwis, który pomaga w budowaniu inteligentnych botów do konwersacji z użytkownikiem. Boty takie pomagają użytkownikom np odpowiadając na żmudne pytania FAQ. Zamiast przeszukiwania kopalni pytań i odpowiedzi użytkownik zadanie pytanie, bot rozpoznaje intencje i zwraca odpowiedź na specyficzne pytanie.
   2. Serwis odpowiada za stworzenie bazy wiedzy to znaczy bazy pytań i odpowiedzi. Oraz następnie poprzez odpytanie odpowiednich endpointów zwraca odpowiedź na konkretne pytanie. Serwis jest szeroko rozwijalny i otwarty na połączenia z innymi serwisami mowy i pisma.
2. **Use cases**
   1. Bardzo aktualnym przykładem jest bot sanepidu odpowiedzialny za bazę pytań i odpowiedzi związaną z COVID.
   2. Bot politechniczny do którego podepniemy baze pytań z dziekanatu, pokazywanie pogody i dojazdy na politechnikę. Bardzo przydatny dla studentów pierwszego roku.
3. **How to**
   1. Tworzymy serwis na portalu Azure lub z użyciem ARM Template. Tworzymy bazę wiedzy i publikujemy ją. Po opublikowaniu dostajemy adres API na który musimy wysyłać zapytania. Jednak przy uzyciu Emulatora BOT możemy utrzymywać stałe połączenie.
   
   2. Pricing - mamy możliwość stworzenia kanału standard albo premium. Obydwie instancje kanału standard mają nielimitowane zapytania ale rózni je szybkość odpowiedzi. Przy kanałach premium i darmowej instancji mamy darmowe 10000 zapytań na miesiac przy płatnej $0.50 za 1000 zapytań.
   
      

# Bot Framework Composer

1. **Intro**

   1. Bot Framework Composer jest to wizualny edytor (Development environment) do tworzenia własnego bota w Bot Framework. Polega on na zasadzie Drag and Drop (Pociągnij i upuść) gdzie z przygotowanych dla nas okienek i usług wybieramy te które nas interesują i łączymy w taki sposób jak chcemy realizując naszą logikę.
   2. Narzędzie pozwala nie tylko budować prostą logikę bota z podstawowymi dialogami i okienkami ale pozwala na integrację z bardziej skomplikowanymi usługami. Z poziomu Composera możemy dodawać rozpoznawanie intencji z integracją z LUISem (Language Understanding). Można ustawiać wyzwalacze bazujące na intencjach lub konkretnych dialogach.
   3. Narzędzie posiada również (LG) Language Generator który może stworzyć zmienną która następnie będzie używana w odpowiedziach i dialogach tworzonych przez bota.

2. **Use cases**

   1. Stworzenie bota podającego nam prognozę pogody obecną oraz przyszłą.
   2. Stworzenie bota rekomendującego filmy lub seriale na wieczór.

3. **How to**

   1. Pobieramy Composera z linku https://github.com/microsoft/BotFramework-Composer/releases/tag/v1.1.1

      Musimy ustalić jakie komponenty będą nam potrzebne i jaki projekt chcemy realizować.

      Szukamy w dialogu po lewej stronie okna programu potrzebnych funkcji przeciągamy na środkowe pole oraz łączymy w sposób zgodny z naszym projektem.

   2. Pricing - Bot Framework Composer jest darmowym narzędziem. 

      
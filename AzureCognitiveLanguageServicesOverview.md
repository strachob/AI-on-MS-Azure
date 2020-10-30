# Azure Content Moderator

1. **Intro**

   1. Serwis Azure Content Moderator jest to narzędzie wykorzystujące uczenie maszynowe i zestaw przygotowanych API służących do moderowania i cenzury zawartości (tekstu, filmów, obrazów).

   2. Serwis ten nie wycina automatycznie kawałków materiału. Nie zamienia przekleństw na ciąg gwiazdek. Zamiast tego wyszukuje słowa lub elementy, które nauczył się że są niewłaściwe lub niepożądane zwraca nam na to uwagę.

      Może on działać w kilku trybach i zwracać uwagę na różne rzeczy:

      * Profanity: czyli sprawdzanie, wyszukiwanie i zwracanie miejsca z złymi słowami lub elementami
      * Classification: Na podstawie całości zawartości zwraca json z przypisanymi wartościami 0-1 dla 3 różnych kategorii
        * 1 - Wyraźnie seksualne lub dorosłe znaczenie
        * 2 - Znaczenie może być uznane za sugestywne lub dojrzałe
        * 3 - Znaczenie może być uznane za ofensywne
      *  PII: Z użyciem tych danych można w prosty sposób zidentyfikować użytkownika. Potencjalnie szkodliwe

2. **Use cases**

   1. Mamy stronę internetową lub aplikacje do której użytkownicy mogą przesyłać filmy - Blokujemy pornografię
   2. Nasz serwis oferuje dołączenie do swojego konta zdjęcia - Blokujemy niechciane zdjęcia (pornografię / otwarte rany / martwe zwierzęta)
   3. Mamy stronę oferującą zostawianie opinii komentarzy pod serwisami i produktami. Zamieniamy złe słowa na ciągi gwiazdek lub zwracamy użytkownikowi uwagę

3. **How to**

   1. Tworzymy zasób na Portalu Azure lub używamy ARM template. Kopiujemy i zapisujemy wygenerowany klucz dla subskrypcji. Następnie w serwisie w którym chcemy użyć Content moderatora używamy API udostępnionego w regionie najbliżej nas i wysyłamy zawartość którą chcemy sprawdzić do tego API w headerze dodając `Ocp-Apim-Subscription-Key` z wcześniej skopiowanym kluczem

   2. Są dwa typy instancji Free oraz standard.

      Pierwsza jest w pełni darmowa lecz ograniczona do 1 transakcji na sekundę i możliwości wykonania 5000 transakcji miesięcznie.

      Wersja Standard to 10 transakcji na sekundę i nielimitowana liczba transakcji miesięcznie w cenach przedstawionych na zdjęciu poniżej

      ![image-20201028215819842](C:\Users\Bartek Strachowski\AppData\Roaming\Typora\typora-user-images\image-20201028215819842.png)



# LUIS

1. **Intro**

   1. LUIS czyli Language Understanding Intelligent Service jest to serwis który wynosi AI na nową ligę. Odpowiada on za rozumienie sensu zdań i budowanie kontekstu w zależności od podanych w zdaniu słów. Za budowanie sprawnej bazy i oznaczanie zdań i sensu słów odpowiedzialny jest jednak użytkownik.

   2. Serwis odpowiada za zrozumienie sensu przesłanego do niego zdania, analizę go i zwrócenie prawdopodobieństw z jakimi zdanie należy do konkretnego kontekstu. Na podstawie tego kontekstu serwis korzystający z LUISa może wygenerować poprawną odpowiedź dla użytkownika.

      LUIS korzysta z 3 głównych aspektów podczas rozumienia języka:

      - **Wypowiedzi:** Wypowiedz to wejście wprowadzone przez użytkownika które wymaga interpretacji.
      - **Intencje:** Intencja reprezentuje akcję lub zadanie które użytkownik planuje wykonać. To cel wyrażony w wypowiedzi.
      - **Encje:** Encja reprezentuje słowo albo frazę w wypowiedzi którą trzeba wydobyć.

      Kluczem jest analiza Wypowiedzi za pomocą zrozumienia specyficznych encji i wyprowadzenia intencji użytkownika.

      Im score bliżej 1 tym większe prawdopodobieństwo zgodności intencji

2. **Use cases**

   1. Bot politechniczny dla młodych studentów. Student pyta o dostęp do dziekanatu albo o dojazd do politechniki i LUIS za pomocą słów "dziekanat" czy "tramwaj" zwraca konkretną intencję.
   2. Bot w banku który w zależności od intencji użytkownika będzie łączył z konkretnym konsultantem lub sam odpowiadał na pytania. 

3. **How to**

   1. * Tworzymy na portalu Azure lub za pomocą ARM template usługę **Language Understanding**.

      * Na stronie Luis.ai logujemy sie za pomocą konta Microsoft i tworzymy aplikację zaznaczają swoją "culture" czyli kraj. 

      * Tworzymy intencję i podajemy przynajmniej 5 przykładowych wypowiedzi dla danej intencji

      * Tworzymy encje i oznaczamy w podanych wypowiedziach konkretne słowa jako nowostworzone encje

      * Trenujemy model LUIS

      * Generujemy endpoint do zapytań i publikujemy jako Web APP

      * Odpytujemy nasz endpoint podając zdanie

      * Poniżej przykładowa odpowiedź LUISa

        {
          "query": "when do you open next?",
          "topScoringIntent": {
            "intent": "GetStoreInfo",
            "score": 0.984749258
          },
          "intents": [
            {
              "intent": "GetStoreInfo",
              "score": 0.984749258
            },
            {
              "intent": "None",
              "score": 0.2040639
            }
          ],
          "entities": []
        }

   2. Również mamy dwa tryby użytkowania:

      * FREE ograniczony do 5 transakcji na sekundę w tym trybie mamy 10000 zapytań tekstowych oraz milion zapytań edycyjnych.
      * Standard to 50 zapytań na sekundę oraz $1.5 za 1000 transakcji tekstowych i $5.50 za 1000 transakcji edycyjnych. 

# Text Analytics API

1. **Intro**
   1. Jest to serwis kognitywny którego celem jest wyciągnięcie z tekstu potrzebnych informacji. Z pomocą tego serwisu można badać język tekstu, wykrywać intencję w tekście, wybierać kluczowe słowa lub frazy oraz wykrywać znane encje.
   2. Serwis potrafi wykrywać uczucia i przypisuje danemu tekstowi wartość pomiędzy 0 a 1. Wartości blisko 1oznaczają pozytywny tekst a wartości blisko 0 negatywny. Mamy możliwość wytrenowania modelu wyspecjalizowanego w medycynie i wtedy możemy klasyfikować terminologię medyczną.
2. **Use cases**
   
   1. W botach dany serwis może rozpoznawać język w zależności tego jak użytkownik się przedstawi. 
   2. Możemy dodać go do bota i zbierać emocje użytkowników na temat produktów.
3. **How to**
   
   1. Tworzymy serwis na portal Azure lub z użyciem ARM template. Pobieramy klucz wygenerowany dla naszego serwisu. Wykonujemy zapytania na `https://[location].api.cognitive.microsoft.com/text/analytics/v2.0/sentiment` z dodaniem naszego klucza w headerze `Ocp-Apim-Subscription-Key`  i dostajemy podobną odpowiedź ![Screenshot of the section number five showing the response content.](https://docs.microsoft.com/en-us/learn/modules/classify-user-feedback-with-the-text-analytics-api/media/7-marker.png)
   
      Ten endpoint służy do wykrywania nastroju więc otrzymaliśmy ocenę w polu score.
   
   2. Pricing  jest podzielony na kilka sekcji. Stworzenie serwisu w kontenerze w darmowej wersji pozwala na 5000 transakcji  miesięcznie ale nie pozwala na rozpoznawanie encji. Duże instancje które pozwalają nam na wszystko zaczynają sie od 75 dolarów miesięcznie i pozwalają na 25 tysięcy transakcji, następnie mam 250 $/m i 100 tyś zapytań, 1000 $/m i 500 tyś zapytań. W najdroższej wersji mamy 5000 dolarów miesięcznie przy 10 mln zapytań.


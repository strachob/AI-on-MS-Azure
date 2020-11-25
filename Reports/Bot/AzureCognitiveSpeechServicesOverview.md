# Transcribe

1. **Intro**

   1. Speech-to-text jest to serwis kognitywny pozwalający na transkrypcję audio w czasie rzeczywistym. Opiera się o użycie uczenia maszynowego.
   2. Serwis ten umożliwia we własnej aplikacji transkrypcję audio z różnych języków. Umożliwione jest to za pomocą API Restowych lub Speech SDK. Microsoft przygotował tak zwany Universal  language model który został wytrenowany z użyciem wewnętrznych danych i jest udostępniony w chmurze. Można trenować modele oparte o własne języki i ich wymowy.

2. **Use cases**

   1. Możemy utworzyć bota dla ludzi którzy mają znaczne wady w dłoniach lub z innych przyczyn  sami nie mogą pisać. Pomagać im w nauce i zdobywaniu informacji.
   2. Serwis do automatycznego robienia notatek z wykładu. Na podstawie audio z wykładu serwis generuje plik tekstowy który możemy dostosować do własnej potrzeby.

3. **How to**

   1. Tworzymy serwis Speech na Azure Portal lub generujemy za pomocą ARM Template. Kopiujemy klucz z usługi który za pomocą którego będziemy się później identyfikować.  

      Jeśli używamy SDK to w C# pobieramy NuGet `Microsoft.CognitiveServices.Speech`a w Pythonie `azure.cognitiveservices.speech` 

      O opisie metod można przeczytać `https://docs.microsoft.com/pl-pl/azure/cognitive-services/speech-service/speech-sdk?tabs=windows%2Cubuntu%2Cios-xcode%2Cmac-xcode%2Candroid-studio`
   
      Serwis ma również możliwość oczekiwania na dane i dopiero kiedy uzyska jakiś input to zacznie transkrybować.
   
      Co ważne formatem obsługiwanym jest **.wav**
   
   2. Pricing - w darmowej wersji mamy jedno zapytanie na raz oraz 5h audio miesięcznie i hostowanie 1 modelu na miesiąc  
   
      W wersji płatnej 20 zapytań na raz, $1 za godzinę audio oraz $1.29 za godzinę hostowania modelu. Przy transkrypcji wielokanałowej $2.10 za godzinę audio. 
   
      

# Synthesize

1. **Intro**

   1. Text - to - Speech jest odwrotnością serwisu opisanego powyżej. Kolekcja zapisanych głosów z których użytkownik może wybierać jest używana do czytania zapisanego tekstu.
   2. Serwis rozpoznaje słowa zapisane w tekście, zapisuje ich znaczenia i przeszukuje swoją bibliotekę gotowych słów i łączy je w zdania w taki sposób aby brzmiały tak jak prawdziwy człowiek.

2. **Use cases**

   1. W nauce gdy niektórzy męczą się czytaniem tekstu suchego. Taki serwis mógłby czytać artykuł przez co lepiej byłoby go zapamiętać. 
   2. Dobrym przykładem był Profesor Hawking który nie mógł nawet pisać a komputer generował za niego jego głos. Dla ludzi którzy nie mogą mówić ale mogą pisać taki system jest idealny. 

3. **How to**

   1. Tworzymy serwis Speech na Azure Portal lub generujemy za pomocą ARM Template. Kopiujemy klucz z usługi który za pomocą którego będziemy się później identyfikować.  

      Jeśli używamy SDK to w C# pobieramy NuGet `Microsoft.CognitiveServices.Speech`a w Pythonie `azure.cognitiveservices.speech` 

      O opisie metod można przeczytać `https://docs.microsoft.com/pl-pl/azure/cognitive-services/speech-service/speech-sdk?tabs=windows%2Cubuntu%2Cios-xcode%2Cmac-xcode%2Candroid-studio`
   
      Podajemy plik tekstowy na wejściu i mamy możliwość generowania audio albo na output albo do pliku audio.
   
      Możemy używać głosów neuronowych aby głos używał emocji podczas czytania. 
   
      Long Text API jest używane przy bardzo długich tekstach
   
   2. Pricing - w darmowej wersji mamy jedno zapytanie na raz oraz:
   
      * W wersji standard 5 milionów znaków na miesiąc
      * W wersji Neural 0.5 miliona znaków na miesiąc
   
      W wersji płatnej 20 zapytań na raz oraz:
   
      * $4 za milion znaków w wersji standard
      * $16 za milion znaków w wersji neural
      * W Preview jest wersja Custom Neural gdzie mozemy własny głos dodawać i wtedy synteza takiego głosu koszkuje $24 dolary za milion znaków

# Translate

1. **Intro**
   1. Tłumaczenie tekstu to serwis który na podstawie podanej ścieżki audio generuje w czasie rzeczywistym ten sam tekst w języku który wybraliśmy. Został stworzony w celach poznawania nowych kultur, łamania granic językowych oraz prowadzenia biznesu za granicą. Serwis potrafi również automatycznie rozpoznawać język w którym się do niego mówi.
   2. Serwis budzi się i nasłuchuje zdania. Czeka aż użytkownik skończy wypowiedź i zrobi pauze. Po krótkiej chwili zdanie zostanie wypowiedziane w języku docelowym. Zdanie tłumaczone jest na bieżąco nawet w chwili gdy użytkownik mówi. Lecz serwis oczekuje na zakończenie zdania aby nie zagłuszać mówiącego. 
2. **Use cases**
   
   1. Idealne dla starszych ludzi którzy nie nauczyli się za dużo języków obcych a są chętni zwiedzać 
   2. Idealne dla ludzi którzy zwiedzają ale akurat nie znają tego języka
   3. Idealne dla ludzi studiujących z obcych krajów którzy szukają wiedzy w innym języku
3. **How to**

   1. Tworzymy serwis Speech na Azure Portal lub generujemy za pomocą ARM Template. Kopiujemy klucz z usługi który za pomocą którego będziemy się później identyfikować.  

      Jeśli używamy SDK to w C# pobieramy NuGet `Microsoft.CognitiveServices.Speech`a w Pythonie `azure.cognitiveservices.speech` 

      O opisie metod można przeczytać `https://docs.microsoft.com/pl-pl/azure/cognitive-services/speech-service/speech-sdk?tabs=windows%2Cubuntu%2Cios-xcode%2Cmac-xcode%2Candroid-studio`

      Jeśli mamy ustawiony domyślny mikrofon to będzie on nasłuchiwał wprowadzenia głosu. 

      Jest również możliwość tłumaczenia na wiele języków jednocześnie.

   2. Pricing - w darmowej wersji mamy jedno zapytanie na raz oraz:

      5h audio na miesiąc

      W wersji płatnej 20 zapytań na raz oraz $2.5 za godzinę audio.


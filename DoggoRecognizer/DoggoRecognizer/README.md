# DoggoRecognizer

### 1. Wstęp

DoggoRecognizer jest prostym serwisem przygotowanym dla ludzi, którzy uwielbiają psy (pjeski) lub nie są pewni czy pies którego widzą należy do jakiejś rasy. Idea serwisu jest bardzo prosta. Wybierz zdjęcie psa, które chcesz sprawdzić a serwis powie Ci jakiej rasy jest pies i dostarczy trochę informacji na temat tej rasy wraz z linkiem do strony gdzie znaleźć można jeszcze więcej informacji.

Na chwilę obecną serwis obsługuje 20 ras, lecz skalowalność systemu jest bardzo duża.

Serwis używa dwóch innych usług:

1. Custom Vision Prediction API - główna usługa na podstawie której serwis został wyuczony. Na postawie wielu zdjęć różnych ras psów nauczony został model kognitywny, który potrafi zaklasyfikować psa do odpowiedniej rasy wraz z jakimś prawdopodobieństwem.

2. Dog API - [The Dog API ](https://documenter.getpostman.com/view/4016432/the-dog-api/RW81vZ4Z) darmowy serwis dostarczający bazowe informacje na temat ras psów tj (ich temperament, średni wzrost czy pochodzenie)

   #### Demo

   Link do dema w serwisie Youtube: [Youtube](https://youtu.be/Bif8qRzqHdk)


### 2. Dostęp do aplikacji

Aplikacja jest stroną internetową więc jest dostępna dla każdego urządzenia z dostępem do internetu.

### 3. Przykładowy przypadek użycia

1. Wchodzimy na stronę
2. Wybieramy zdjęcie psa z dysku 
3. Klikamy "Upload"
4. Widzimy zdjęcie które przesłaliśmy oraz informacje na temat rasy psa.

### 4. Jak projekt został stworzony?

1. Stworzenie projektu w .NET Core 3.1. ASP.NET Core MVC
2. Utworzenie projektu Custom Vision na stronie [Custom Vision](https://www.customvision.ai/)
3. Zebranie zdjęć psów z podziałem na rasy. Łącznie 20 ras - około 400 zdjęć
4. Integracja Custom Vision z projektem. [Microsoft Docs](https://docs.microsoft.com/pl-pl/azure/cognitive-services/custom-vision-service/quickstarts/image-classification?tabs=visual-studio&pivots=programming-language-csharp)
5. Przygotowanie bazy wiedzy (linków oraz id) przygotowanych ras zgodnie z Dog API
6. Integracja serwisu Dog API z projektem



### 5.  Jak go postawić u siebie?

1. Zainstalować środowisko .Net Core w wersji 3.1

2. Pobrać repozytorium z kodem

3. Utworzyć usługę CustomVision i dodać zdjęcia psów z własnego archiwum lub z folderu *Doggos*

4. Wytrenować model oraz udostępnić Predictive API URL

5. Otworzyć plik appsettings.json i zmienić dane zamieszczone poniżej

   ```json
   {
      "PredictionUrl": "<Here the url for predictive API>",
      "PredictionKey": "<Here the key for predictive API>"
   }
   ```

6. Uruchomić lokalnie i zacząć działanie

7. [Opcjonalnie] otworzyć plik doggosettings.json i dodać 

   ```json
   {
       "Name": "Vizsla", //The name of the bread must be the same as the Custom Vision tag
       "Id": 251, // The id of the bread from Dog API. Can put 0 is not existant
       "WikiPage": "https://dogtime.com/dog-breeds/vizsla" // dogtime link to the bread
   }
   ```

   






# Custom Vision

1. **Intro**

   Custom Vision jest to usługa wykorzystująca bogaty silnik Cognitive Services. Jak sama nazwa wskazuje usługa zapewnia nam jakąś dostosowaną dla użytkownika funkcjonalność. W tym przypadku mamy możliwość do usługi załączyć przygotowane przez nas zbiory zdjęć i specjalnie je oznaczyć wewnątrz usługi. Następnie model Machine Learning jest trenowany na podstawie zdjęć oraz przypisanych im znaczników. 

   Po publikacji takiego modelu możemy wysyłać do niego kolejne zdjęcia i zwróci on przypisane do tego zdjęcia znaczniki wraz z prawdopodobieństwem z którym się one tam znajdują. 

2. **Use cases**

   1. Aplikacja dla grzybiarzy która pomaga rozpoznać typ grzyba i opisuje go na podstawie zdjęcia.
   2. Aplikacja dla fanatyków motoryzacji do rozpoznawania Marki, modelu i generacji samochodu na podstawie zdjęcia.

3. **How to**

   Usługa znajduje się pod [Linkiem](https://www.customvision.ai/). Po wejściu na stronę i utworzeniu zasobu oraz projektu możemy zacząć dodawać oraz oznaczać zdjęcia zgodnie z tym jak je przygotowaliśmy. Zdjęć tych powinno być odpowiednio dużo aby model skutecznie się nauczył. Następnie trenujemy model i publikujemy go. Po opublikowaniu przechodzimy do Prediction API i szukamy URL do API.

   Teraz możemy zacząć już używać przygotowanego modelu. Wysyłamy zapytania do Prediction API z kodu lub z użyciem narzędzia typu Postman.

   Możemy też używać Training API w celu Batchowego przyłania wielu zdjęć od razu z przypisanymi tagami. 

   Przykładowy Json takiego zapytania.

   ```json
   {
     "images": [
       {
         "url": "{url to image #1}",
         "tagIds": [ "{tag-id}" ]
       }
     ],
     "tagIds": [
        "{tag-id}"
     ]
   }
   ```

   
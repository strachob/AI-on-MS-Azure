# USE CASE
Classify if the breast tumor is malignant or benign. By it's size and placement. The data shown below.

![data-excel](.\data-excel.png)

Data is explained in **breast-cancer-wisconsin.names** file in the located in the repository.

# Learn AutoMl with UI

I clicked through the UI guideline of making AutoML. It it really simple. I selected Classification as the target method and Class column as column. All other properties and options were autoselected for optimization. Selected machine cost $0.09/h.  The experiment run multiple child model parings to figure out which algorithm has the best accuracy for the job.

![ui-run](E:\MAGISTERSKIE\SEMESTR2\Azure\Laby\AI-on-MS-Azure\Reports\AutoML\ui-run.png)



And after over 1h later the results came with [VotingEnsemble](https://ml.azure.com/experiments/id/2406bef4-357d-4e25-91f5-f56a73749add/runs/AutoML_ba4a282a-5b8d-46f9-ba48-7749b199b962_43?wsid=/subscriptions/da29bcc9-497c-44b3-95aa-169e164600f6/resourceGroups/AutoML-RG/providers/Microsoft.MachineLearningServices/workspaces/AutoML-Cancer&tid=3b50229c-cd78-4588-9bcf-97b7629e2f0f#model) algorithm as the best one with accuracy equal 97.422%

# First Steps

1. Log into Azure subscription and create Azure Machine Learning resource.

   ![automl-resource](E:\MAGISTERSKIE\SEMESTR2\Azure\Laby\AI-on-MS-Azure\Reports\AutoML\automl-resource.png)

2. Open Machine Learning Studio

3. Import data from **breast-cancer-wisconsin.csv**

4. Create new Notebook and attach a compute module to it.

5. Authenticate to Azure by running code below and follow the instructions.

   ```python
   from azureml.core import Workspace, Dataset
   
   # Get Workspace defined in by default config.json file
   ws = Workspace.from_config()
   ```

6. 
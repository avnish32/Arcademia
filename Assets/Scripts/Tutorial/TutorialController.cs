using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ResearchArcade;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI msgText, continueText;

    [SerializeField]
    private GameObject assmtQPointer, livesAndScorePtr,
        socsDeskPtr;

    [SerializeField] 
    private GameObject slidePickup, reportPgPickup1, reportPgPickup2,
        videoClipPickup1, videoClipPickup2, codePickup;

    [SerializeField]
    private GameObject speedReducer, progressResetter;

    [SerializeField]
    private GameObject speedBooster, assmtCompleter, extraLife;

    [SerializeField]
    AssignmentController_Tut assmtController;

    [SerializeField]
    PickupSpawner_Tut pickupSpawner;

    [SerializeField]
    SceneController sceneController;

    private List<Action> actions;
    private List<GameObject> allPickups; 
    private int nextActionToExec = 0;
    private bool canContinue = false;
    private GameObject lastPickupSpawned;

    public Action OnPickupCollected, OnPickupNotCollected, OnAssmtSubmitted;

    // Start is called before the first frame update
    void Start()
    {
        allPickups = new List<GameObject>() {slidePickup, reportPgPickup1, reportPgPickup2,
        videoClipPickup1, videoClipPickup2, codePickup };        

        assmtQPointer.SetActive(false);
        livesAndScorePtr.SetActive(false);
        socsDeskPtr.SetActive(false);

        actions = new List<Action>() { Welcome, AssmtComponents, PresSlide,
        ReportPgs, Lives, TwoAssmts, ChangingAssmtUseful, ChooseCorrectAssmt,
        ProgressResetter, SpeedBooster, ExtraLife, AssmtCompleter,
        PauseAndExit, ThatsIt, LoadMainMenu};
        

        canContinue = true;
        ContinueTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (ArcadeInput.Player1.B.Down)
        {
            ContinueTutorial();
        }
    }

    private void ContinueTutorial()
    {
        if (!canContinue)
        {
            return;
        }
        actions[nextActionToExec].Invoke();
        nextActionToExec++;
    }

    private void Welcome()
    {
        assmtController.SpawnFirstAssignment();
        msgText.text = "Welcome to Assessment Training! On the right are the assessments that you need to complete.";
    }

    private void AssmtComponents()
    {
        assmtQPointer.SetActive(true);
        msgText.text = "Each assessment needs you to collect certain stuff. For example, this one requires 2 presentation slides " +
            "and 5 report pages before its timer of 2 minutes runs out.";
    }

    private void PresSlide()
    {
        CanUserContinue(false);
        assmtQPointer.SetActive(false);
        pickupSpawner.SpawnPickup(slidePickup);
        msgText.text = "Here's 2 presentation slides. Use the Player 1 joystick and walk over it to collect before it disappears!";
        OnPickupCollected = PresSlideUpdatedOnAssmt;
        OnPickupNotCollected = () =>
        {
            pickupSpawner.SpawnPickup(slidePickup);
        };
    }

    private void PresSlideUpdatedOnAssmt()
    {
        OnPickupCollected = OnPickupNotCollected = null;
        assmtQPointer.SetActive(true);
        msgText.text = "The assessment status updates to reflect your progress.";
        CanUserContinue(true);
    }

    private void ReportPgs()
    {
        CanUserContinue(false);
        assmtQPointer.SetActive(false);
        pickupSpawner.SpawnPickup(reportPgPickup1);
        pickupSpawner.SpawnPickup(reportPgPickup2);
        msgText.text = "You might need to collect the same type of item multiple times to complete the assessment, such as these report pages.";

        OnPickupCollected = () =>
        {
            if (assmtController.IsAssmtCompleted(assmtController.GetActiveAssmt()))
            {
                FirstAssmtCompleted();
            } else
            {
                pickupSpawner.SpawnPickup(reportPgPickup2);
            }
        };

        OnPickupNotCollected = () =>
        {
            pickupSpawner.SpawnPickup(reportPgPickup2);
        };
    }

    private void FirstAssmtCompleted()
    {
        CanUserContinue(false);
        OnPickupCollected = OnPickupNotCollected = null;
        socsDeskPtr.SetActive(true);
        msgText.text = "Great! The assessment is now completed. It will keep beeping to remind you to submit. Walk over to the SoCS Submission desk to submit it.";
        OnAssmtSubmitted = Timer;
    }

    private void Timer()
    {
        socsDeskPtr.SetActive(false);
        OnAssmtSubmitted = null;
        CanUserContinue(true);
        msgText.text = "Good job. Keep in mind that if you fail to submit an assessment before its timer runs out, you'll lose one life.";
    }

    private void Lives()
    {
        livesAndScorePtr.SetActive(true);
        msgText.text = "Your lives and score are displayed here.";
    }

    private void TwoAssmts()
    {
        CanUserContinue(true);
        livesAndScorePtr.SetActive(false);
        assmtController.SpawnSecondAssignment();
        assmtController.SpawnThirdAssignment();
        msgText.text = "There are a couple of new assessments in the list. You can choose which assessment to work on using Player 1's 'A' key.";
    }

    private void ChangingAssmtUseful()
    {
        msgText.text = "But how is choosing the active assessment useful? You see, the items you collect will affect only the active assessment.";
    }

    private void ChooseCorrectAssmt()
    {
        CanUserContinue(false);
        pickupSpawner.SpawnPickup(videoClipPickup1);
        msgText.text = "In this case, since the first assessment does not require a video clip, but the second one does, " +
            "it would be wise to collect this video clip pickup while the " +
            "second assessment is highlighted. Try it.";
        OnPickupCollected = () =>
        {
            var activeAssmtFields = assmtController.GetActiveAssmt().fields;
            foreach (var field in activeAssmtFields)
            {
                if (field.field == E_AssignmentFields.VIDEO_DURATION)
                {
                    CompleteBothAssmts();
                    return;
                }
            }
            pickupSpawner.SpawnPickup(videoClipPickup2);
        };

        OnPickupNotCollected = () =>
        {
            pickupSpawner.SpawnPickup(videoClipPickup2);
        };
    }

    private void CompleteBothAssmts()
    {
        CanUserContinue(false);
        OnPickupCollected = OnPickupNotCollected = null;

        msgText.text = "Nice. This way you can ensure that the collected item reaches the correct assessment. Try to complete and submit both these assessments now.";
        foreach (var pickup in allPickups)
        {
            pickup.GetComponent<AssmtPickup_Tut>().DisablePtr();
        }
        
        pickupSpawner.StartSpawningPickups(allPickups);

        OnAssmtSubmitted = () =>
        {
            if (assmtController.GetAssmtQ().Count <=0)
            {
                SpeedReducer();
            }
        };
    }

    private void SpeedReducer()
    {
        CanUserContinue(true);
        OnAssmtSubmitted = null;

        pickupSpawner.StopSpawningPickups();
        lastPickupSpawned = pickupSpawner.SpawnPickup(speedReducer);
        msgText.text = "Occasionally, you will see some red pickups like this one. This has a pink tortoise on it (poor choice of colors, I know), and it " +
            "will reduce your speed for a short while.";
    }

    private void ProgressResetter()
    {
        CanUserContinue(true);
        if (lastPickupSpawned != null)
        {
            Destroy(lastPickupSpawned);
        }
        lastPickupSpawned = pickupSpawner.SpawnPickup(progressResetter);
        msgText.text = "This is another baddie, which will reset any progress made on your active assessment. Try to avoid these red pickups.";
    }

    private void SpeedBooster()
    {
        if (lastPickupSpawned != null)
        {
            Destroy(lastPickupSpawned);
        }
        lastPickupSpawned = pickupSpawner.SpawnPickup(speedBooster);
        msgText.text = "It's not all bad, though. There are some powerups as well, like this one, which will boost your speed temporarily.";
    }

    private void ExtraLife()
    {
        if (lastPickupSpawned != null)
        {
            Destroy(lastPickupSpawned);
        }
        lastPickupSpawned = pickupSpawner.SpawnPickup(extraLife);
        msgText.text = "And this one, which adds one life up to a maximum of 5 lives.";
    }

    private void AssmtCompleter()
    {
        if (lastPickupSpawned != null)
        {
            Destroy(lastPickupSpawned);
        }
        lastPickupSpawned = pickupSpawner.SpawnPickup(assmtCompleter);
        assmtController.SpawnFourthAssignment();
        msgText.text = "Finally, this powerup, perhaps the strongest one, will complete the active assessment for you.";
    }

    private void PauseAndExit()
    {
        msgText.text = "You can press 'Start' at any time to pause the game, and 'Exit' To exit the game.";
    }

    private void ThatsIt()
    {
        msgText.text = "And that is it for the tutorial! Good luck on your assessment training, hope you have fun!";
    }

    private void LoadMainMenu()
    {
        sceneController.LoadMainMenu();
    }

    private void CanUserContinue(bool canThey)
    {
        canContinue = canThey;
        continueText.enabled = canThey;
    }
}

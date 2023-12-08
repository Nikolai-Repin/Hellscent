
// To get the thing to work, simply drag in the UI Manager into the Scene (It's located in Assets/Resources/Prefabs/UI)


// Bugs:
// 1) Occasionally when you exit the "play" state, the console starts spamming NullReferenceException: SerializedObject of SerializedProperty has been Disposed.
// It does not affect gameplay in any way, but it's annoying.
// I have no clue what causes it, but you can fix it by toggling the active state of the Health Outline.
// If any of you have any idea how you could fix this, please tell me.

// 2) Could not get takeDamage() to actually make the player take damage
// If you have a working implementation of takeDamage(), adding UIManager.updateHealth(); should get the ui to update.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //[SerializeField] private Camera camera;
    
    [SerializeField] private Image ManaBar;

    [SerializeField] private Image heartContainer;
    [SerializeField] private List<Image> heartContainerOutlines = new List<Image>();
    [SerializeField] private List<Image> heartContainerInsides = new List<Image>();

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
        updateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        ManaBar.fillAmount = controller.GetManaPercent();
    }

    // Updates the amount of health you have displayed
    public void updateHealth() {
        if (heartContainerOutlines.Count < controller.GetMaxHealthAmount()) {
            for (int i = heartContainerOutlines.Count; i < controller.GetMaxHealthAmount(); i++) {
                heartContainerOutlines.Add(heartContainer);
                heartContainerOutlines[heartContainerOutlines.Count - 1] = Instantiate(heartContainerOutlines[heartContainerOutlines.Count - 1], new Vector3((-850f + (heartContainerOutlines.Count - 1)*150), 450f, 0f), Quaternion.identity);
                heartContainerOutlines[heartContainerOutlines.Count - 1].name = "Heart Container " + (heartContainerOutlines.Count);
                heartContainerOutlines[heartContainerOutlines.Count - 1].transform.SetParent(gameObject.transform.GetChild(0), false);
                heartContainerOutlines[heartContainerOutlines.Count - 1].gameObject.SetActive(true);

                // Getting the child of the heart container outlines (the heart "insides") and putting them in a list.
                heartContainerInsides.Add(heartContainerOutlines[i].transform.GetChild(0).GetComponent<Image>());

                heartContainerInsides[heartContainerOutlines.Count - 1].name = "Heart Container Insides " + (heartContainerOutlines.Count);
            }
        }

        // janky code that "fills" every heart container to the appropriate amount.
        for (int i = heartContainerOutlines.Count - 1; i >= controller.GetHealthAmount(); i--) {
            heartContainerInsides[i].fillAmount = 0;
        }

        int index;
        for (index = 0; index <= controller.GetHealthAmount() - 1; index++) {
            heartContainerInsides[index].fillAmount = 1;
            //Debug.Log(index);
        }

        if (index < controller.GetHealthAmount()) {
            heartContainerInsides[index].fillAmount = 0.5f;
        }

    }

    public void UpdateEntityHealthBar(Entity entity) {
        Debug.Log("Enemy Health Updated");
        entity.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = (entity.GetHealthAmount()/entity.GetMaxHealthAmount());
        //entity.healthBar.fillAmount = (entity.GetHealthAmount()/entity.GetMaxHealthAmount());
        
    }

}
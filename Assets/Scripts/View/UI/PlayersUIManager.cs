using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Com.Nextplease.IWT;

public class PlayersUIManager : MonoBehaviour
{
    public static readonly int marketIndex = 3;
    public GameObject[] PlayerAvatars;
    public Image HealthBar;
    public Animator coreAnimator;

    private readonly float lerpSpeed = 1f;
    private readonly int fullHealth = MarketManager.StartingCastleHealth;
    private int currentHealth = MarketManager.StartingCastleHealth;
    private int prevSelectedAvatar;
    private int currSelectedAvatar;
    private List<Animator> playerAvatarAnimators;

    void OnEnable()
    {
        EventManager.Instance.AddListener<CameraPanEvent>(OnCameraPan);
        EventManager.Instance.AddListener<DamageTakenEvent>(OnDamageTaken);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveListener<CameraPanEvent>(OnCameraPan);
        EventManager.Instance.RemoveListener<DamageTakenEvent>(OnDamageTaken);
    }

    void Awake()
    {
        playerAvatarAnimators = new List<Animator>();
        foreach (GameObject avatar in PlayerAvatars)
        {
            playerAvatarAnimators.Add(avatar.GetComponent<Animator>());
        }
    }

    void Update()
    {
        if (currentHealth == fullHealth)
            return;

        float newFillAmount = (float)currentHealth / fullHealth;
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, newFillAmount, Time.deltaTime * lerpSpeed);

        if (HealthBar.fillAmount < 0.3f)
        {
            HealthBar.color = Color.Lerp(HealthBar.color, Color.red, Time.deltaTime * lerpSpeed);
        }
    }

    void OnDamageTaken(DamageTakenEvent e)
    {
        StartCoroutine(TakeDamage(e.currentHealth));
    }

    void OnCameraPan(CameraPanEvent e)
    {
        int playerNum = (int)e.targetView == -1 ? marketIndex : (int)e.targetView;
        prevSelectedAvatar = currSelectedAvatar;
        currSelectedAvatar = playerNum;

        DeselectPreviousAvatar();
        SelectAvatar(playerNum);
    }

    private void SelectAvatar(int playerNum)
    {
        if (currSelectedAvatar != marketIndex)
        {
            playerAvatarAnimators[playerNum].Play("Select");
        }
    }

    private void DeselectPreviousAvatar()
    {
        if (prevSelectedAvatar != marketIndex)
        {
            playerAvatarAnimators[prevSelectedAvatar].Play("Deselect");
        }
    }

    IEnumerator TakeDamage(int newHealth)
    {
        coreAnimator.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(0.5f);
        currentHealth = newHealth;
    }
}

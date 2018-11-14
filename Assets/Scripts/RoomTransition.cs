using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour {

    [Header("Type Settings")]
    public Transition transitionType;

    [Header("Destination")]
    public Transform targetTrigger;
    public RoomMaster targetRoomMaster;

    [Header("Exit Settings")]
    public Direction exitDirection;

    [Header("Scene Settings")]
    public bool targetIsScene;
    public SceneSpawn sceneTarget;

    [Header("UI")]
    public FadeUI darkness;

    private bool playerStandsInTrigger = false;
    private bool doublePressPrevention = false;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (transitionType == Transition.None)
            return;

        if (col.tag == Tags.Player.ToString())
        {
            player = col.transform;
            switch (transitionType)
            {
                case Transition.WalkInTrigger:
                    if (targetIsScene)
                    {
                        StartCoroutine(ChangeSceneAsync());
                        return;
                    }
                    StartCoroutine(DoTransition(targetTrigger, targetRoomMaster, player));
                    break;
                case Transition.ButtonPress:
                    playerStandsInTrigger = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == Tags.Player.ToString())
        {
            playerStandsInTrigger = false;
        }
    }

	void Update ()
    {
        if (transitionType == Transition.None)
            return;

		if (playerStandsInTrigger && transitionType == Transition.ButtonPress && Globals.GetButtonDown("Action") && !doublePressPrevention)
        {
            if (player.GetComponent<PreventInput>().InputProhibited)
                return;

            doublePressPrevention = true;

            if (targetIsScene)
            {
                StartCoroutine(ChangeSceneAsync());
                return;
            }

            StartCoroutine(DoTransition(targetTrigger, targetRoomMaster, player));
        }
	}

    public IEnumerator ChangeSceneAsync()
    {       
        Time.timeScale = 0;
        player.GetComponent<PreventInput>().InputProhibited = true;
        darkness.visible = true;

        DontDestroy.GetInstance().GetComponent<StoreSceneInfo>().sceneSpawn = sceneTarget;

        yield return new WaitForSecondsRealtime(0.5f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneTarget.sceneName.ToString());

        while (!op.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator DoTransition(Transform targetTrigger, RoomMaster targetRoomMaster, Transform player)
    {
        Time.timeScale = 0;

        player.GetComponent<PreventInput>().InputProhibited = true;
        player.GetComponent<CharacterHealth>().ForceStopIFrames();
        targetRoomMaster.gameObject.SetActive(true);
        darkness.visible = true;

        yield return new WaitForSecondsRealtime(0.5f);

        Vector3 offset = Vector3.zero;

        if (targetTrigger.GetComponent<RoomTransition>().transitionType == Transition.WalkInTrigger)
        {
            switch (targetTrigger.GetComponent<RoomTransition>().exitDirection)
            {
                case Direction.Right:
                    offset = Vector3.right;
                    break;
                case Direction.Left:
                    offset = Vector3.left * 0.7f;
                    break;
                case Direction.None:
                    offset = Vector3.zero;
                    break;
                case Direction.Down:
                    break;
                case Direction.Up:
                    break;

            }
        }

        float damp = targetRoomMaster.mainCamera.damp;
        targetRoomMaster.mainCamera.damp = 0;

        targetRoomMaster.InstantiateRoom();
        player.position = new Vector3(targetTrigger.position.x, targetTrigger.position.y - 1, targetTrigger.position.z) + offset;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.75f);

        darkness.visible = false;
        player.GetComponent<PreventInput>().InputProhibited = false;
        doublePressPrevention = false;
        targetRoomMaster.SnapParallax();

        if (targetTrigger.GetComponent<RoomTransition>().exitDirection == Direction.Left)
        {
            player.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (targetTrigger.GetComponent<RoomTransition>().targetRoomMaster)
        {
            targetTrigger.GetComponent<RoomTransition>().targetRoomMaster.gameObject.SetActive(false);
        }


        targetRoomMaster.mainCamera.damp = damp;
    }

}

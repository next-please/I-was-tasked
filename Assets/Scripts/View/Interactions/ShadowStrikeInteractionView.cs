using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStrikeInteractionView : InteractionView
{
    public GameObject SmokePoof;
    private ShadowStrikeSkill shadowStrike;

    public GameObject smokeSrc;
    public GameObject smokeDest;
    void Start()
    {
        shadowStrike = interaction as ShadowStrikeSkill;
    }

    public override void CleanUpInteraction()
    {
        var caster = shadowStrike.caster;
        var pieceView = caster.GetPieceView();

        Vector3 pos = ViewManager.CalculateTileWorldPosition(shadowStrike.targetTile);
        pos.y = pieceView.transform.position.y;

        smokeSrc = Instantiate(SmokePoof, shadowStrike.attackSource, Quaternion.identity);
        smokeDest = Instantiate(SmokePoof, pos, Quaternion.identity);
        smokeSrc.transform.parent = transform;
        smokeDest.transform.parent = transform;
        StartCoroutine(WaitToTeleport());
    }

    IEnumerator WaitToTeleport()
    {
        yield return new WaitForSeconds(0.2f);
        var pieceView = shadowStrike.caster.GetPieceView();
        Vector3 pos = ViewManager.CalculateTileWorldPosition(shadowStrike.targetTile);
        float y = transform.position.y;
        pieceView.transform.position = new Vector3(pos.x, y, pos.z);
        if (!shadowStrike.target.IsDead())
        {
            pieceView.LookAtTile(shadowStrike.target.GetCurrentTile());
        }
        yield return WaitForCleanUp();
    }

    IEnumerator WaitForCleanUp()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "ConstantsSO", menuName = "Scriptable Objects/ConstantsSO")]
public class ConstantsSO : ScriptableObject
{
    [Header("���μ���")]
    public int Rows;
    public int Columns;
    [Header("�ִϸ��̼��� ���� �ð�")]
    public float AnimationDuration;
    [Header("�̵� �ִϸ��̼��� �ּ� ���� �ð�")]
    public float MoveAnimationMinDuration;
    [Header("���� �ִϸ��̼��� ���� �ð�")]
    public float ExplosionDuration;
    [Header("���� �ִϸ��̼��� �ּ� ���� �ð�")]
    public float WaitBeforePotentialMatchesCheck;
    [Header("��Ʈ ���� �ִϸ��̼��� ������ ����")]
    public float OpacityAnimationFrameDelay;
    [Header("��ġ�� ���ֵǴ� �ּ� ��ġ, ���ʽ� ���� ��")]
    public int MinimumMatches;
    public int MinimumMatchesForBonus;
    [Header("���� �ð� ����")]
    public float GameTimeLimit;
}

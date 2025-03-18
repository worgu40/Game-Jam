[System.Serializable]
public class PlayerData
{
    public float positionX, positionY, positionZ;
    public float health;
    public int currentLevel;
    public bool isFacingRight;
    public bool? rightJump; 
    public float levelTimer;
    public float minutes;

    public PlayerData(Player player)
    {
        positionX = player.transform.position.x;
        positionY = player.transform.position.y;
        positionZ = player.transform.position.z;
        health = player.health;
        currentLevel = player.currentLevel;
        isFacingRight = player.isFacingRight;
        rightJump = player.rightJump;
        levelTimer = player.levelTimer;
        minutes = player.minutes;
    }
}

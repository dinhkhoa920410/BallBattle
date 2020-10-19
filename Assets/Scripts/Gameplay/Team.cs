public class Team
{
    public const int DRAW = -1;
    public const int TEAM_A = 0;
    public const int TEAM_B = 1;
    public const int ROLE_ATTACKER = 0;
    public const int ROLE_DEFENDER = 1;
    public const float MAX_ENERGY = 6.0f;
    public const float ENERGY_REGEN = 0.5f;
    public const int COST_ATTACKER = 2;
    public const int COST_DEFENDER = 3;
    public int role;
    public int color;
    public float energy;
    public float energyRegen;
    public int winTimes;
    public string playerName;
    public int energyCost;
    public int side;

    public Team(int role, int side, int color)
    {
        this.role = role;
        this.side = side;
        this.color = color;
        energy = 0;
        energyRegen = ENERGY_REGEN;
        winTimes = 0;
        if(role == ROLE_ATTACKER){
            playerName = "TEAM_A";
            energyCost = COST_ATTACKER;
        }else{
            playerName = "TEAM_B";
            energyCost = COST_DEFENDER;
        }
    }

    public string getInfo()
    {
        return playerName + "  " + (role==ROLE_ATTACKER?"(Attacker)":"(Defender)");
    }

    public string getStringRole(){
        return role==ROLE_ATTACKER?"<Attacker>":"<Defender>";
    }

    public void initNewRound(){
        if(role == ROLE_ATTACKER){
            energyCost = COST_ATTACKER;
        }else{
            energyCost = COST_DEFENDER;
        }
        energy = 0;
    }
}

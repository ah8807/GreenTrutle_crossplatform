namespace GreenTrutle_crossplatform.scene.Objects;

public class Score: Text
{
    private int points;
    
    public Score() 
    {
        points = 0;
        this.text = "Score: "+points;
    }

    public void incScore()
    {
        this.points++;
        update();
    }
    public void decScore()
    {
        this.points--;
        update();
    }

    private void update()
    {
        this.text = "Score: " + points;
    }

    public void Reset()
    {
        this.points = 0;
        update();
    }
}
namespace GreenTrutle_crossplatform.scene.Objects;

public class Score: Text
{
    private int _points;

    public int points
    {
        set { _points = value; update(); }
        get { return _points; }
    }

    public Score() 
    {
        _points = 0;
        this.text = "Score: "+_points;
    }

    public void incScore()
    {
        this._points++;
        update();
    }
    public void decScore()
    {
        this._points--;
        update();
    }

    private void update()
    {
        this.text = "Score: " + _points;
    }

    public void Reset()
    {
        this._points = 0;
        update();
    }
}
using UnityEngine;

public class GameManager :SingleTon<GameManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float elapsedTime = 0;
    float interval = 1f;
    int frameCount = 0;
    float fps;


    private void Update()
    {
        
        CalculateAvarageFpsAndShow();
    }

    public void CalculateAvarageFpsAndShow()
    {
        elapsedTime += Time.unscaledDeltaTime;
        frameCount++;
        fps += 1.0f / Time.deltaTime;
        if (elapsedTime >= interval)
        {
            UIManager.instance.SetFpsBar(Mathf.Floor((fps / frameCount) * 100) / 100);
            elapsedTime = 0;
            frameCount = 0;
            fps = 0;
        }
       
    }



}

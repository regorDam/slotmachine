# KC-Games

[![KC](https://games.kintoncloud.com/assets/img/PoweredBy.png)](https://kintoncloud.com)



  <a href="https://user-images.githubusercontent.com/9436924/114547553-dcc9be80-9c5e-11eb-9d95-a4c17e2c8856.mp4">
    <img align="right" width="300" src="https://user-images.githubusercontent.com/9436924/114551000-1ef4ff00-9c63-11eb-8cd6-785154e80ce9.gif">
  </a>


Descargar APK:
https://cutt.ly/2ve5v8G

## Documentación
Este documento pretende explicar un poco la estructura y algunas tomas de decisiones.

Se han creado algunos scripts útiles a niveles generales como el Init para limitar los FPS, ya que no existe ninguna necesidad de forzar el hardware al máximo y más en dispositivos móviles que tienden a calentarse.

Otra clase interesante es Console, sirve para activar y desactivar los logs. Ya que en una versión definitiva o de release es altamente recomendable no gastar recursos en mostrar mensajes en consola.

Para la lectura de la API se ha creado una clase APITime junto a un controlador APIController.
```sh
[Serializable]
public class APITime
{
    public string abbreviation;
    public string client_ip;
    public string datetime;
}
```

En este caso se ha desarrollado esta API usando Laravel, pero no vamos a entrar más detalle. La URL de destino es: https://games.kintoncloud.com/time

Resultado de la petición a la URL:
```sh
{"datetime":"2021-04-13T12:13:10.577384Z","timezone":"Europe\/Madrid"}
```
El proyecto es bastante simple, así que solo comentaremos aquellas partes "un poco" más complejas.

Para la realización del efecto de rotación de los 3 slots se ha la clase Scroller. A continuación se muestra el método Update.

````sh
void Update()
{
  if (isStopped) return;

  float newPosition = Time.deltaTime * scrollSpeed * 5;
  transform.position = transform.position + Vector3.up * newPosition;

  transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f);

  if(transform.position.y < minY)
  {
    transform.position = new Vector3(startPosition.x, maxY, startPosition.z);
  }
}
````
La siguiente clase importante es GameController, encargado de gestionar la escena de juego, en este caso es una única escena, pero que funciona como tres. Ya que en el menú tenemos las opciones de "Juego 1", "Juego 2" y "Juego 3". Los recursos necesarios para cada una de ellas se instancian según decide el GameManager. El caso particular y más interesante (en el que se encuentra el juego en sí) es "Juego 1". Realmente es bastante simple, ya que no requiere prácticamente lógica, solo tenemos el desencadenante al clicar el botón y chequear el resultado. Y activan sus respectivos efectos de sonido o texto.

````sh
...
...
...
void Awake()
{
    levelManager = LevelManager.Instance;
    Scroller[] unsortedList;
    switch (levelManager.CurrentGameScene)
    {
        case 1:
            Instantiate(Resources.Load("Prefabs/SlotMachine"), new Vector3(0,-1,0), Quaternion.identity);
            unsortedList = GameObject.FindObjectsOfType<Scroller>();
            scrollers = unsortedList.OrderBy(go => go.name).ToList();
            winGO = GameObject.FindGameObjectWithTag("Win");
            winGO.SetActive(false);
            spinBtn = GameObject.FindObjectOfType<ButtonController>();
            break;
        case 2:
            Instantiate(Resources.Load("Prefabs/Prefabgame2"), Vector3.zero, Quaternion.identity);
            break;
        case 3:
            Instantiate(Resources.Load("Prefabs/Prefabgame3"), Vector3.zero, Quaternion.identity);
            break;
        default:
            unsortedList = GameObject.FindObjectsOfType<Scroller>();
            scrollers = unsortedList.OrderBy(go => go.name).ToList();
            break;
    }

    audioSource = GetComponent<AudioSource>();

}
...
...
...
public void OnClickSpinBtn(ButtonController go)
{
    Console.Log("Spin");
    winGO.SetActive(false);
    spinBtn = go;
    spinBtn.isEnabled = false;
    audioSource.clip = stopSpin;
    int count = 0;
    foreach (Scroller scroller in scrollers)
    {
        float time = 0.5f * count;

        count++;

        StartCoroutine(StartSpin(time, scroller));
    }
    StartCoroutine(EnableSpinBtn());
}
...
...
...
private void CheckResult()
{
    Debug.Log("CheckResult");
    bool isStopped = false;
    foreach (Scroller scroller in scrollers)
    {
        if (scroller.isStopped)
            isStopped = true;
        else
            isStopped = false;
    }
    Console.Log(isStopped+"");

    if(isStopped)
    {
        if(scrollers[0].GetResult() == scrollers[1].GetResult() && scrollers[0].GetResult() == scrollers[2].GetResult())
        {
            audioSource.clip = win;
            audioSource.Play();
            winGO.SetActive(true);
        }
    }
}
````

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class zombieScript : MonoBehaviour
{
    // объявление преобразования нашей цели (куда будет двигаться навигационный агент) и нашего навигационного агента (в данном случае нашего зомби)
    private Transform goal;
    private NavMeshAgent agent;

    //  Инициализация
    void Start()
    {

        //создание ссылок
        goal = Camera.main.transform;
        agent = GetComponent<NavMeshAgent>();
        //устанавливаем назначение навигационной сетки равным положению основной камеры (наш персонаж от первого лица)
        agent.destination = goal.position;
        //запускаем анимацию ходьбы
        GetComponent<Animation>().Play("Z_Walk");
    }


    void OnTriggerEnter(Collider col)
    {
        //сначала отключим коллайдер зомби, чтобы не могло произойти множественных столкновений
        GetComponent<CapsuleCollider>().enabled = false;
        // уничтожаем пулю
        Destroy(col.gameObject);
        //останавливаем зомби от движения вперед, установив его пункт назначения в текущее положение
        agent.destination = gameObject.transform.position;
        //останавливаем анимацию ходьбы и воспроизводим откат назад
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().Play("Z_FallingBack");
        //уничтожить этого зомби за шесть секунд
        Destroy(gameObject, 6);
        //создание нового зомби
        GameObject zombie = Instantiate(Resources.Load("zombie", typeof(GameObject))) as GameObject;

        //устанавливаем координаты для нового вектора 3
        float randomX = UnityEngine.Random.Range(-12f, 12f);
        float constantY = .01f;
        float randomZ = UnityEngine.Random.Range(-13f, 13f);
        //устанавливаем позицию зомби равной этим новым координатам
        zombie.transform.position = new Vector3(randomX, constantY, randomZ);

        // если зомби окажется менее или равным 3 единицам сцены от камеры, мы не сможем его снять
        // так что продолжим перемещать зомби, пока он не отойдет больше чем на 3 единицы сцены.
        while (Vector3.Distance(zombie.transform.position, Camera.main.transform.position) <= 3)
        {

            randomX = UnityEngine.Random.Range(-12f, 12f);
            randomZ = UnityEngine.Random.Range(-13f, 13f);

            zombie.transform.position = new Vector3(randomX, constantY, randomZ);
        }

    }

}
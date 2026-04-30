using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _container;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<Bullet> _pool;
    private Bullet _currentBullet;

    public Bullet CurrnetItem => _currentBullet;

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(
           createFunc: () => CreateItem(),
           actionOnGet: (item) => ActionOnGet(item),
           actionOnRelease: (item) => item.gameObject.SetActive(false),
           actionOnDestroy: (item) => Destroy(item.gameObject),
           collectionCheck: true,
           defaultCapacity: _capacity,
           maxSize: _maxSize);
    }

    public void Spawn()
    {
        _currentBullet = _pool.Get();
        _currentBullet.transform.position = _spawnPoint.position;
        _currentBullet.transform.rotation = _spawnPoint.rotation;
        _currentBullet.gameObject.SetActive(true);
        _currentBullet.Initialize(_spawnPoint.right);
    }

    private Bullet CreateItem()
    {
        var item = Instantiate(_prefab, _container);

        return item;
    }

    private void ActionOnGet(Bullet bullet)
    {
        _currentBullet = bullet;
        bullet.Deactivated += ReleaseItem;
    }

    protected virtual void ReleaseItem(Bullet bullet)
    {
        bullet.Deactivated -= ReleaseItem;

        _pool.Release(bullet);
    }
}
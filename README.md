# RedisSentinel
## 建立Redis Cluster 跟 Redis Sentinel
```
docker build -f Redis\Node\Dockerfile -t redis-node:custom .
docker build -f Redis\Sentinel\Dockerfile -t redis-sentinel:custom .

# 建立容器私有網段
docker network create --subnet=10.100.0.0/16 redis

# 建立所有Master跟Slave關係
docker run -d --network redis --ip 10.100.0.2 --name rm1 -p 7001:6379 redis-node:custom
docker run -d --network redis --ip 10.100.0.3 --name rs1 -p 7002:6379 redis-node:custom redis-server --slaveof rm1 6379
docker run -d --network redis --ip 10.100.0.4 --name rm2 -p 7003:6379 redis-node:custom
docker run -d --network redis --ip 10.100.0.5 --name rs2 -p 7004:6379 redis-node:custom redis-server --slaveof rm2 6379
docker run -d --network redis --ip 10.100.0.6 --name rm3 -p 7005:6379 redis-node:custom
docker run -d --network redis --ip 10.100.0.7 --name rs3 -p 7006:6379 redis-node:custom redis-server --slaveof rm3 6379

# 建立哨兵
docker run -d --network redis --name redis-senrinel1 redis-sentinel:custom
docker run -d --network redis --name redis-senrinel2 redis-sentinel:custom
docker run -d --network redis --name redis-senrinel3 redis-sentinel:custom

# 挑其中一台進去建立叢集
redis-cli --cluster create 10.100.0.2:6379 10.100.0.3:6379 10.100.0.4:6379 10.100.0.5:6379 10.100.0.6:6379 10.100.0.7:6379 --cluster-replicas 1
```
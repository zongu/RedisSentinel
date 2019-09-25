FROM redis:5.0.5
EXPOSE 6379 16379
COPY rediscluster.conf /etc/redis/rediscluster.conf
ENTRYPOINT redis-server /etc/redis/rediscluster.conf
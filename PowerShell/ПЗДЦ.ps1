на проде для работы с API погоды требуется прописать в кубе 
```
kubectl create secret generic weatherapi-secret \
  --from-literal=WEATHER_API_KEY=25696eaa04a54bdd8cc132631251805 \
  --namespace=default
```

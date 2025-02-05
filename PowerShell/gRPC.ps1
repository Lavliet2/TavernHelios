###Local
#MenuService
grpcurl -plaintext localhost:5064 list    
grpcurl -plaintext localhost:5064 describe TavernHelios.GrpcContract.MenuService
grpcurl -plaintext -d '{}' localhost:5064 TavernHelios.GrpcContract.MenuService.GetAllDishes


grpcurl -plaintext localhost:5065 list    

grpcurl -plaintext localhost:5065 describe TavernHelios.GrpcContract.ReservationService
grpcurl -plaintext -d '{}' localhost:5065 TavernHelios.GrpcContract.ReservationService

###Kubernetes
#ReservationService
grpcurl -plaintext 178.72.83.217:32042 list   
grpcurl -plaintext 178.72.83.217:32042 describe TavernHelios.GrpcContract.ReservationService

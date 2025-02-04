grpcurl -plaintext localhost:5064 list    

grpcurl -plaintext localhost:5064 describe TavernHelios.GrpcContract.MenuService
grpcurl -plaintext -d '{}' localhost:5064 TavernHelios.GrpcContract.MenuService.GetAllDishes

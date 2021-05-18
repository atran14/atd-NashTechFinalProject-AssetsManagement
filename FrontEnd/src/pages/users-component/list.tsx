import { useEffect } from 'react'
import { UserService } from '../../services/UserServices'

export function ListUsers() {
  useEffect(() => {
    (async () => {
      let userServices = UserService.getInstance()
      let usersPagedResponse = await userServices.getUsers()

      console.log(usersPagedResponse);
    })();
  })

  return <div>This is where one view users</div>
}

import { createContext } from 'react';

const UserContext = createContext(
{
    isLoggedIn: false,
    setLoggedInState: () => {},
    username: null,
    setUsername: () => {}
})

export default UserContext;
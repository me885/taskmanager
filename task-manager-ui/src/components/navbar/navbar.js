import { Box } from '@mui/material';
import "./navbar.css";
import SigninButton from './signinButton';
import SignupButton from './signupButton';
import LogoutButton from './logoutButton';
import { useContext } from 'react';
import UserContext from '../../UserContext';


const Navbar = () => {   

    const currentUser = useContext(UserContext)
    
    return (
        <Box className="navbar">
            <Box className="left-content">
                TaskManager
            </Box>

            <Box className="right-content">
                {currentUser.isLoggedIn ? 
                <LogoutButton /> :
                <><SignupButton /><SigninButton /></> }
            </Box>
        </Box>
    )
}

export default Navbar;
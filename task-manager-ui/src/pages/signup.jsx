import {Button, Container, TextField, Stack} from '@mui/material';
import { useNavigate } from "react-router-dom";
import UserContext from '../UserContext';
import { useContext } from 'react';

import "./signup.css";


const SignUp = () => {  

    const {setLoggedInState} = useContext(UserContext)

    const navigate = useNavigate(); 
    const handleExisitingAccount = () =>
    {
        navigate("/signin");
    }

    const handleSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);

        const signupObject = {
            name: formData.get("username"),
            password: formData.get("password"),
        }

        console.log(signupObject);

        await fetch("https://taskmanager-todo.azurewebsites.net/auth/register", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(signupObject),
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
        .then(async () => {
            document.getElementById("signup-form").reset()

            await fetch("https://taskmanager-todo.azurewebsites.net/auth/getToken", {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(signupObject),
            })
            .then(async (response) => {
                if(response.status === 200)
                {
                    localStorage.setItem("jwt", (await response.json()).token);
                    setLoggedInState(true);
                    document.getElementById("signup-form").reset()
                    navigate("/");
                }
            })
        })
    };
  
    return (
        <Container className="signup-box">
            <Stack
            id="signup-form"
            className="signup-stack"
            component="form"
            onSubmit={handleSubmit}
            spacing={2}
            noValidate
            autoComplete="off">
                <TextField 
                    id="username" 
                    label="Username" 
                    name="username"
                    variant="outlined"
                    className="signup-textbox" />
                <TextField 
                    id="password" 
                    label="Password" 
                    name="password"
                    variant="outlined"
                    type="password" 
                    className="signup-textbox" />
                <Button 
                    className="signup-button"
                    type="submit"
                    variant="contained"
                    color="primary">
                    Create Account
                </Button>
                <Button 
                    className="signup-button"
                    onClick={handleExisitingAccount}
                    variant="outlined"
                    color="primary">
                    I already have an account
                </Button>
            </Stack>
      </Container>
    )
}

export default SignUp;
import {Button, Container, TextField, Stack} from '@mui/material';


import "./signup.css";


const SignUp = () => {  
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
        .then(response => {
            document.getElementById("signup-form").reset()
            console.log("success");
        })
        .catch(error => {
            console.log(error)
            console.log("failed");
        })
    };
  
    return (
        <Container className="signup-box">
            <Stack
            id="signupForm"
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
            </Stack>
      </Container>
    )
}

export default SignUp;
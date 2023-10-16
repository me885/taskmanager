import { useContext, useEffect } from 'react';
import UserContext from '../UserContext';
import { useNavigate } from 'react-router';
import TaskTable from '../components/taskmanagement/taskTable';
import CreateTaskButton from '../components/taskmanagement/createTaskButton';
import { Container } from '@mui/material';

const Home = () => {
  const currentUser = useContext(UserContext)
  const navigate = useNavigate(); 

  useEffect(() => {
    if(!currentUser.isLoggedIn)
    {
      navigate("/signin")
    }
  })

  if(!currentUser.isLoggedIn)
  {
    return;
  }

  return (
    <Container>
      <div style={{textAlign:'right', padding:24}}>
        <CreateTaskButton />
      </div>
      <TaskTable />
    </Container>
  );
}

export default Home;
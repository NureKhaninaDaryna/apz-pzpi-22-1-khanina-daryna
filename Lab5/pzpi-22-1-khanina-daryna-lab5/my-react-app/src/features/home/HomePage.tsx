import {Container, Header, Segment, Button, Icon} from "semantic-ui-react";
import {useStore} from "../../app/stores/store.ts";
import {observer} from "mobx-react-lite";
import LoginForm from "../users/LoginForm.tsx";
import RegisterForm from "../users/RegisterForm.tsx";
import {Link} from "react-router-dom";

export default observer(function HomePage() {
   const {userStore, modalStore} = useStore();

   return (
      <Segment inverted textAlign="center" vertical className="masthead">
         <Container text>
            <Header as="h1" inverted>
               <Icon name="food" size="massive" style={{ marginBottom: 12 }} />
               DineMetrics
            </Header>
            {userStore.isLoggedIn ? (
               <>
                  <Header as="h2" inverted content="Welcome to DineMetrics" />
                  <Button as={Link} to="/eateries" size="huge" inverted>
                     Go to Eateries
                  </Button>
               </>
            ) : (
               <>
                  <Button onClick={() => modalStore.openModal(<LoginForm />)} size="huge" inverted>
                     Login
                  </Button>
                  <Button onClick={() => modalStore.openModal(<RegisterForm />)} size="huge" inverted>
                     Register
                  </Button>
               </>
            )}
         </Container>
      </Segment>
   );
})

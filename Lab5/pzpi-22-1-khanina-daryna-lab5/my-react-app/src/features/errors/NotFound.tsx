import {Button, Header, Icon, Segment} from "semantic-ui-react";
import {Link} from "react-router-dom";

export default function NotFound() {
   return (
      <Segment placeholder>
         <Header icon>
            <Icon name="search" size="large" />
            Oops - we've looked everywhere but could not be found.
         </Header>
         <Segment.Inline>
            <Button as={Link} to={'/eateries'}>
               Return to eateries page
            </Button>
         </Segment.Inline>
      </Segment>
   )
}
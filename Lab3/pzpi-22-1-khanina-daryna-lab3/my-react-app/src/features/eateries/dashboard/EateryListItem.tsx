import {Icon, Card, Label, Button} from "semantic-ui-react";
import {Eatery, EateryType} from "../../../app/models/eatery.ts";
import {Link} from "react-router-dom";
import {useStore} from "../../../app/stores/store.ts";
import ConfirmDialogBody from "../../../app/common/modals/ConfirmDialogBody.tsx";
import {observer} from "mobx-react-lite";
import {hasAccess, ManagementName, PermissionAccess} from "../../../app/stores/permissionStore.ts";

interface Props {
   eatery: Eatery;
}

export default observer(function EateryListItem({ eatery }: Props) {
   const { modalStore, eateryStore, userStore } = useStore();

   return (
      <Card fluid>
         <Card.Content>
            <Card.Header>
               <Icon name="food" color="orange" /> {eatery.name}
            </Card.Header>
            <Card.Meta>
               <Icon name="map marker alternate" /> {eatery.address}
            </Card.Meta>
            <Label color="teal" ribbon>
               {EateryType[eatery.type]}
            </Label>
         </Card.Content>

         <Card.Content>
            <p>
               <strong>
                  <Icon name="calendar check outline" /> Opening Day:
               </strong> {eatery.openingDay}
            </p>
            <p>
               <strong>
                  <Icon name="clock outline" /> Operating Hours:
               </strong> {eatery.operatingHours}
            </p>
            <p>
               <strong>
                  <Icon name="users" /> Max Capacity:
               </strong> {eatery.maximumCapacity} people
            </p>
            <p>
               <strong>
                  <Icon name="thermometer half" /> Temp Threshold:
               </strong> {eatery.temperatureThreshold}°C
            </p>
         </Card.Content>

         {
            userStore.user && hasAccess(userStore.user.role, ManagementName.AnalyticsManagement, PermissionAccess.Full) && (
               <Card.Content extra>
                  <Button
                     as={Link}
                     to={`/eateries/${eatery.id}/edit`}
                     color="blue"
                     icon
                     labelPosition="left"
                  >
                     <Icon name="edit" />
                     Update
                  </Button>
                  <Button
                     color="red"
                     icon
                     labelPosition="left"
                     onClick={() => modalStore.openModal(
                        <ConfirmDialogBody
                           title="Delete Eatery"
                           content={`Are you sure you want to delete "${eatery.name}"?`}
                           onConfirm={() =>
                              eateryStore.deleteEatery(eatery.id).then(modalStore.closeModal)}
                           onCancel={modalStore.closeModal}
                        />
                     )}
                  >
                     <Icon name="trash" />
                     Delete
                  </Button>
               </Card.Content>
            )
         }
      </Card>
   );
});


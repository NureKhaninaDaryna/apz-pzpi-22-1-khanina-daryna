import {observer} from "mobx-react-lite";
import {useStore} from "../../../app/stores/store.ts";
import EateryListItem from "./EateryListItem.tsx";

export default observer(function EateryList() {
   const { eateryStore } = useStore();
   const eateries = Array.from(eateryStore.eateryRegistry.values());

   return (
      <>
         {eateries.map((eatery) => (
            <EateryListItem eatery={eatery} key={eatery.id} />
         ))}
      </>
   );
});
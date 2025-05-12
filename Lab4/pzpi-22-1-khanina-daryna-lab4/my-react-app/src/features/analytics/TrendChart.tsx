import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { Segment, Dropdown, DropdownProps, Message } from "semantic-ui-react";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts";

export default observer(function TrendChart() {
   const { analyticsStore, eateryStore } = useStore();
   const { trends, facilityId, loadTrendsAnalysis } = analyticsStore;
   const { eateries, loadEateries } = eateryStore;

   useEffect(() => {
      loadEateries();
   }, [loadEateries]);

   const handleEateryChange = async (_: any, data: DropdownProps) => {
      await loadTrendsAnalysis(data.value as number);
   };

   const eateryOptions = eateries.map(e => ({
      key: e.id,
      text: e.name,
      value: e.id
   }));

   return (
      <Segment>
         <h3>Trend Analysis</h3>
         <Dropdown
            placeholder="Select Eatery"
            fluid
            selection
            options={eateryOptions}
            onChange={handleEateryChange}
            value={facilityId}
            style={{ marginBottom: '1rem' }}
         />

         {trends?.length === 0 && facilityId ? (
            <Message warning content="No trend data available for this eatery." />
         ) : (
            <ResponsiveContainer width="100%" height={300}>
               <LineChart data={trends}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="date" tickFormatter={(d) => new Date(d).toLocaleDateString()} />
                  <YAxis />
                  <Tooltip labelFormatter={(d) => new Date(d).toLocaleDateString()} />
                  <Line type="monotone" dataKey="averageValue" stroke="#8884d8" strokeWidth={2} />
               </LineChart>
            </ResponsiveContainer>
         )}
      </Segment>
   );
});

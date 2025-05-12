import { useState } from "react";
import { Button, Form, Header, Segment, Statistic } from "semantic-ui-react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import {useStore} from "../../app/stores/store.ts";
import {observer} from "mobx-react-lite";

export default observer(function DashboardMetrics() {
   const [from, setFrom] = useState<Date | null>(null);
   const [to, setTo] = useState<Date | null>(null);
   const { analyticsStore } = useStore();

   const handleSubmit = async () => {
      if (!from || !to) return;

      await analyticsStore.loadDashboard(from, to);
   };

   return (
      <Segment>
         <Header as="h2" content="Dashboard Metrics" />
         <Form onSubmit={handleSubmit}>
            <Form.Group widths="equal">
               <Form.Field>
                  <label>From</label>
                  <DatePicker
                     selected={from}
                     onChange={(date) => setFrom(date)}
                     dateFormat="yyyy-MM-dd"
                     maxDate={new Date()}
                     placeholderText="Start date"
                     value={analyticsStore.fromDate?.toDateString()}
                  />
               </Form.Field>
               <Form.Field>
                  <label>To</label>
                  <DatePicker
                     selected={to}
                     onChange={(date) => setTo(date)}
                     dateFormat="yyyy-MM-dd"
                     maxDate={new Date()}
                     placeholderText="End date"
                     value={analyticsStore.toDate?.toDateString()}
                  />
               </Form.Field>
            </Form.Group>
            <Button type="submit" primary loading={analyticsStore.loading} disabled={!from || !to}>
               Load Metrics
            </Button>
         </Form>

         {analyticsStore.dashboardInfo && (
            <Statistic.Group widths="two" style={{ marginTop: "1rem" }}>
               <Statistic>
                  <Statistic.Value>{analyticsStore.dashboardInfo.averageTemperature.toFixed(1)}°C</Statistic.Value>
                  <Statistic.Label>Average Temperature</Statistic.Label>
               </Statistic>
               <Statistic>
                  <Statistic.Value>{analyticsStore.dashboardInfo.totalMetrics}</Statistic.Value>
                  <Statistic.Label>Total Metrics</Statistic.Label>
               </Statistic>
            </Statistic.Group>
         )}
      </Segment>
   );
})

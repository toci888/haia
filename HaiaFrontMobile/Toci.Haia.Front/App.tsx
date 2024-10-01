import * as React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs';
import Icon from 'react-native-vector-icons/Ionicons';
import FacebookScreen from './Facebook';
import CommentScreen from './Comment';
import MainScreen from './Main';

import AllCommentsScreen from './screens/AllCommentsScreen';
import MostLikedJokesScreen from './screens/MostLikedJokesScreen';
import FriendRequestsScreen from './screens/FriendRequestsScreen';
import LiveStreamScreen from './screens/LiveStreamScreen';

// Create a Stack Navigator
const Stack = createNativeStackNavigator();

const Tab = createMaterialTopTabNavigator();

function MyTabs() {
  return (
    <Tab.Navigator
      initialRouteName="AllComments"
      tabBarOptions={{
        activeTintColor: '#fff',
        style: { backgroundColor: '#6200ee' },
        showIcon: true,
        indicatorStyle: { backgroundColor: '#fff' },
        labelStyle: { fontSize: 8 },
      }}
    >
      <Tab.Screen
        name="AllComments"
        component={AllCommentsScreen}
        options={{
          tabBarLabel: 'Wszystkie Komentarze',
          tabBarIcon: ({ color }) => <Icon name="chatbox-ellipses-outline" color={color} size={24} />,
        }}
      />
      <Tab.Screen
        name="MostLikedJokes"
        component={MostLikedJokesScreen}
        options={{
          tabBarLabel: 'Najbardziej Lubiane Żarty',
          tabBarIcon: ({ color }) => <Icon name="thumbs-up-outline" color={color} size={24} />,
        }}
      />
      <Tab.Screen
        name="FriendRequests"
        component={FriendRequestsScreen}
        options={{
          tabBarLabel: 'Zaproszenia do Znajomych',
          tabBarIcon: ({ color }) => <Icon name="people-outline" color={color} size={24} />,
        }}
      />
      <Tab.Screen
        name="LiveStream"
        component={LiveStreamScreen}
        options={{
          tabBarLabel: 'Live Stream',
          tabBarIcon: ({ color }) => <Icon name="videocam-outline" color={color} size={24} />,
        }}
      />
    </Tab.Navigator>
  );
}

//const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator initialRouteName="Main">
      <Stack.Screen name="Home" component={MyTabs} options={{ headerShown: false }} />
      
      </Stack.Navigator>
    </NavigationContainer>
  );

//   <Stack.Screen 
//           name="Main" 
//           component={MainScreen} 
//           options={{ title: 'Main Section' }} 
//         />
//         <Stack.Screen 
//           name="Facebook" 
//           component={FacebookScreen} 
//           options={{ title: 'Facebook Login' }} 
//         />
//         <Stack.Screen 
//           name="Comment" 
//           component={CommentScreen} 
//           options={{ title: 'Comment Section' }} 
//         />
}

import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../../domains/global/models/User';
import { mockUser1 } from '../../common/mocks/mockUser';

interface UserState {
  currentUser: User | null;
}

const initialState: UserState = {
  currentUser: mockUser1,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setUser(state, action: PayloadAction<User | null>) {
      state.currentUser = action.payload;
    },
    clearUser: (state) => {
      state.currentUser = null;
    },
  }
});

export const userReducer = userSlice.reducer;
export const userAction = userSlice.actions;



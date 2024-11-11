export interface Post {
  id: number;
  groupId: number;
  userId: number;
  user: {
    id: number;
    username: string;
  }
  content: string;
  createdAt: string;
  group: {
    id: number;
    name: string;
    description: string;
  }
  categoryId: number;
  category: {
    id: number;
    name: string;
  }
  comments: {
    
  }
}